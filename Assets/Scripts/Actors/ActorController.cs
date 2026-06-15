using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class ActorController : MonoBehaviour
{
    // --- Members ---
    [Header("--- Actor Settings ---")]
    public bool actionsEnabled = true;
    private Vector2 m_lookDir = Vector2.right;  
    public Vector2 lookDir
    {
        get => m_lookDir;
        protected set => m_lookDir = value.normalized;
    }

    protected ActorMove m_move;
 
    public List<ActorAction> actions { get; protected set; }
    public Animator animator { get; protected set; }
    public CoinBag coinBag { get; protected set; }

    [SerializeField] LayerMask m_enemyLayer;
    public LayerMask enemyLayer
    {
        get => m_enemyLayer;
        protected set => m_enemyLayer = value;
    }

    [SerializeField] bool m_enableLookDirGizmo = true;
    [SerializeField] AnimationClip m_idleClip;
    [SerializeField] AnimationClip m_deadClip;

    protected Rigidbody2D m_rb;
    protected AnimatorOverrideController m_animOverrideController;

    // Stats, Actors may not have stats, but if they do, they will
    // use them for different things depending on the type of stat.
    public StatsController stats { get; protected set; }
    public HitPoints hitpoints { get; protected set; }

    private string m_idleAnimClipName = "Actor-Idle";
    private string m_deadAnimParamName = "isDead";
    private string m_deadAnimClipName = "Actor-Dead";
    private DeadAnimSMB m_deadEndSMB;
    private bool m_allowFlip = true;


    // --- Events ---
    public event Action OnDestroy;


    // --- Methods ---
    protected virtual void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        Debug.Assert(m_rb != null, "ActorController: Rigidbody2D component is missing.");

        animator = GetComponent<Animator>();
        Debug.Assert(animator != null, "ActorController: Animator component is missing.");

        // NOTE: It's ok if an actor cannot move or doesn't have hp (cannot take baseDamage).
        m_move = GetComponent<ActorMove>();

        // NOTE: AnimationClips are changeable at runtime, so we need to use an
        // AnimatorOverrideController to override the clips in the animator controller.
        m_animOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        if (animator != null) {
            animator.runtimeAnimatorController = m_animOverrideController;
        }

        // Initialize the list of actions with the actions attached to this actor.
        actions = new List<ActorAction>(GetComponents<ActorAction>());

        // Initialize the coin bag.
        coinBag = new CoinBag();

        // Initialize the StatsController.
        stats = new StatsController(this);
        hitpoints = stats.GetStat<HitPoints>();
    }

    protected virtual void OnEnable()
    {
        enemyLayer = m_enemyLayer;

        SubscribeEvents();
    }

    protected virtual void OnDisable()
    {
        UnsubscribeEvents();
    }

    protected virtual void Start()
    {
        // Set idle animation clip if we have an animator and an idle clip
        if (animator != null && m_idleClip != null) {
            if (!GameManager.UpdateAnimClip(m_animOverrideController, m_idleAnimClipName, m_idleClip)) {
                Debug.LogWarning($"Failed to set idle animation clip for {gameObject.name}. Make sure the animator has a state named '{m_idleAnimClipName}' with an AnimationClip assigned.");
            }
        }

        // Set dead animation clip if we have an animator and a dead clip
        if (animator != null && m_deadClip != null) {
            if (!GameManager.UpdateAnimClip(m_animOverrideController, m_deadAnimClipName, m_deadClip)) {
                Debug.LogWarning($"Failed to set dead animation clip for {gameObject.name}. Make sure the animator has a state named '{m_deadAnimClipName}' with an AnimationClip assigned.");
            }
        }

        // Initialize hitpoints using the StatsController.
        if (stats != null) {
            hitpoints = stats.GetStat<HitPoints>();
        }

        // During initialization (Awake/OnEnable/Start) we don't subscribe to HitPoints
        // because ActorController.OnEnable() runs before StatsController.Awake(), so
        // hitpoints would be null at that point. Instead, we subscribe to HitPoints in Start().
        SubscribeHPZeroEvent();
    }

    protected virtual void Update()
    {
        // Actions take precedence over movement when it comes to determining lookDir.
        // If we're performing an action, look in the target of the action.
        bool lookDirUpdated = false;
        if (actionsEnabled) {

            //TODO: We should look at the last action performed, not just the first action.
            foreach (var action in actions) {
                if (action.isBeingPerformed) {
                    lookDir = action.actionDir;
                    lookDirUpdated = true;
                    break;
                }
            }
        }
        
        if (!lookDirUpdated && m_move != null) {

            // If moving this frame, update lookDir to match moveDir. Otherwise
            // just keepCasting looking in the same target when idle.
            if (m_move.moveDir != Vector2.zero) {
                lookDir = m_move.moveDir;
                lookDirUpdated = true;
            }
        }

        if (FlipNeeded()) {
            Flip();
        }
    }

    // An easy way for other modules to update the animation clips.
    public bool UpdateAnimClip(string animClipName, AnimationClip newAnimClip)
    {
        return GameManager.UpdateAnimClip(m_animOverrideController, animClipName, newAnimClip);
    }

    public void Die()
    {
        // Disable the collider to prevent further interactions
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null) {
            collider.enabled = false;
        }

        // Disable movement and actions
        if (m_move != null && m_move.enabled) {
            m_move.movementEnabled = false;
        }

        actionsEnabled = false;

        animator.SetBool(m_deadAnimParamName, true);
    }

    public void TakeDamage(float damage)
    {
        if (hitpoints != null) {
            hitpoints.TakeDamage((int)damage);
        }
    }

    public bool ActorIsEnemy(ActorController actor)
    {
        // Check if the actor is on the enemy layer
        return (enemyLayer.value & (1 << actor.gameObject.layer)) > 0;
    }

    public T GetAttribute<T>() where T : Attribute
    {
        if (stats != null) {
            foreach (Stat stat in stats) {
                if (stat is T) {
                    return stat as T;
                }
            }
        }
        return null;
    }

    public void PickUp(PickUpController pickUp)
    {
        // Switch which type of item we've been told to pick up.
        Item item = pickUp.item;
        if (item is Coin) {
            if (coinBag != null) {
                coinBag.coinAmount += (item as Coin).value;

                // NOTE: This may trigger a pickup animation.
                pickUp.isPickedUp = true;
            }
        }
        else if (item is Gear) {
            Debug.Log($"Picked up gear: {item.itemName}. This is a placeholder for future gear handling logic.");
            pickUp.isPickedUp = true;
        }
        else {
            Debug.Log($"Collided with an item of type {item.GetType().Name} that we don't know how to handle. Leaving it there.");
        }
    }


    // --- Helper Methods ---
    private bool FlipNeeded()
    {
        // Looking right and facing left
        if (lookDir.x > 0f && transform.localScale.x < 0f) {
            return true;
        }
        // Looking left and facing right
        else if (lookDir.x < 0f && transform.localScale.x > 0f) {
            return true;
        }

        return false;
    }

    private void Flip()
    {
        if (m_allowFlip) {
            transform.localScale = new Vector3(
                -transform.localScale.x,
                transform.localScale.y,
                transform.localScale.z);

            lookDir = -lookDir;
        }
    }

    private void SubscribeEvents()
    {
        // Susbcribe to DeadAnimSMB.OnDeadAnimEnd
        if (animator != null) {
            foreach (var behaviour in animator.GetBehaviours<DeadAnimSMB>()) {
                m_deadEndSMB = behaviour;
                m_deadEndSMB.OnDeadAnimEnd += DeadAnimEndHandler;
            }
        }

        // Subscribe to HP reaching its minimum value (usually 0). Trigger death.
        SubscribeHPZeroEvent();
    }

    private void UnsubscribeEvents()
    {
        if (m_deadEndSMB != null) {
            m_deadEndSMB.OnDeadAnimEnd -= DeadAnimEndHandler;
            m_deadEndSMB = null;
        }

        UnsubscribeHPZeroEvent();
    }

    private void SubscribeHPZeroEvent()
    {
        if (hitpoints != null) {
            hitpoints.OnValueMinimum += Die;
        }
    }

    private void UnsubscribeHPZeroEvent()
    {
        if (hitpoints != null) {
            hitpoints.OnValueMinimum -= Die;
        }
    }


    // --- Event Handlers ---
    private void DeadAnimEndHandler()
    {
        OnDestroy?.Invoke();
        Destroy(gameObject);
    }

    // --- Collision Handling ---
    //protected virtual void OnCollisionEnter2D(Collision2D collision)
    //{
    //    // Check if we collided with a pick up.
    //    PickUpController pickUp = collision.gameObject.GetComponent<PickUpController>();
    //    if (pickUp != null) {
    //        Item item = pickUp.item;

    //        // NOTE: As new types of Items are developed, they need to be controlled here.
    //        // We collided with a coin.
    //        if (item is Coin) {
    //            coinBag.coinAmount += (item as Coin).value;

    //            // NOTE: This may trigger a pickup animation.
    //            pickUp.isPickedUp = true;
    //        }
    //        // We don't know what type of item we collided with. Just leave it there.
    //        else {
    //            Debug.Log($"Collided with an item of type {item.GetType().Name} that we don't know how to handle. Leaving it there.");
    //        }
    //    }
    //}


    // --- Gizmos ---
    private void OnDrawGizmosSelected()
    {
        // Draw a line indicating the look moveDir
        if (m_enableLookDirGizmo) {
            Gizmos.color = Color.blue;
            Vector3 lookDirection = new Vector3(lookDir.x, lookDir.y, 0f);
            Gizmos.DrawLine(transform.position, transform.position + lookDirection);
        }
    }
}
