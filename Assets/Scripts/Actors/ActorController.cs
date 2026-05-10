using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class ActorController : MonoBehaviour
{
    // --- Members ---
    [Header("--- Actor Settings ---")]
    public bool actionsEnabled = true;
    public bool movementEnabled {
        get => move != null && move.movementEnabled;
        set {
            if (move != null) {
                move.movementEnabled = value;
            }
            else {
                Debug.LogWarning($"ActorController: Attempting to set movementEnabled but no ActorMove component found on {gameObject.name}.");
            }
        }
    }
    private Vector2 m_lookDir = Vector2.right;
    public Vector2 lookDir
    {
        get => m_lookDir;
        protected set => m_lookDir = value.normalized;
    }

    public ActorMove move { get; protected set; }
    public List<ActorAction> actions { get; protected set; }
    public Animator animator { get; protected set; }

    [SerializeField] LayerMask m_enemyLayer;
    public LayerMask enemyLayer
    {
        get => m_enemyLayer;
        protected set => m_enemyLayer = value;
    }

    [SerializeField] AnimationClip m_idleClip;
    [SerializeField] AnimationClip m_deadClip;

    protected Rigidbody2D m_rb;
    protected AnimatorOverrideController m_animOverrideController;

    private string m_idleAnimClipName = "Actor-Idle";
    private string m_deadAnimParamName = "isDead";
    private string m_deadAnimClipName = "Actor-Dead";
    private DeadAnimSMB m_deadEndSMB;
    private bool m_allowFlip = true;
    

    // --- Methods ---
    protected virtual void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        Debug.Assert(m_rb != null, "ActorController: Rigidbody2D component is missing.");

        animator = GetComponent<Animator>();
        Debug.Assert(animator != null, "ActorController: Animator component is missing.");

        // NOTE: It's ok if an actor cannot move.
        move = GetComponent<ActorMove>();

        // NOTE: AnimationClips are changeable at runtime, so we need to use an
        // AnimatorOverrideController to override the clips in the animator controller.
        m_animOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        if (animator != null) {
            animator.runtimeAnimatorController = m_animOverrideController;
        }

        // Initialize the list of actions with the actions attached to this actor.
        actions = new List<ActorAction>(GetComponents<ActorAction>());
    }

    protected virtual void OnEnable()
    {
        enemyLayer = m_enemyLayer;

        // Susbcribe to DeadAnimSMB.OnDeadAnimEnd
        if (animator != null) {
            foreach (var behaviour in animator.GetBehaviours<DeadAnimSMB>()) {
                m_deadEndSMB = behaviour;
                m_deadEndSMB.OnDeadAnimEnd += DeadAnimEndHandler;
            }
        }
    }

    protected virtual void OnDisable()
    {
        // Unsubscribe from DeadAnimSMB.OnDeadAnimEnd
        if (m_deadEndSMB != null) {
            m_deadEndSMB.OnDeadAnimEnd -= DeadAnimEndHandler;
            m_deadEndSMB = null;
        }
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
        
        if (!lookDirUpdated && move != null) {

            // If moving this frame, update lookDir to match moveDir. Otherwise
            // just keepGoing looking in the same target when idle.
            if (move.moveDir != Vector2.zero) {
                lookDir = move.moveDir;
                lookDirUpdated = true;
            }
        }

        if (FlipNeeded()) {
            Flip();
        }
    }

    protected virtual void FixedUpdate()
    {
        if (move != null && move.movementEnabled && move.moveDir != Vector2.zero) {
            m_rb.MovePosition(
                (Vector2)transform.position + move.moveDir.normalized * move.moveSpeed * Time.fixedDeltaTime);
        }
    }

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
        if (move != null) {
            move.movementEnabled = false;
        }

        actionsEnabled = false;

        animator.SetBool(m_deadAnimParamName, true);
        //animator.Play(m_deadStateName);
    }


    // --- Event Handlers ---
    private void DeadAnimEndHandler()
    {
        Destroy(gameObject);
    }


    // --- Gizmos ---
    private void OnDrawGizmosSelected()
    {
        // Draw a line indicating the look moveDir
        Gizmos.color = Color.blue;
        Vector3 lookDirection = new Vector3(lookDir.x, lookDir.y, 0f);
        Gizmos.DrawLine(transform.position, transform.position + lookDirection);
    }
}
