using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class ProjectileController : MonoBehaviour
{
    // --- Members ---
    [Header("--- Projectile Settings ---")]
    public float damage = 1f;

    [HideInInspector] public ActorController thrower;
    [HideInInspector] public Vector2 moveDir
    {
        get => m_move != null ? m_move.moveDir : Vector2.zero;
        set
        {
            if (m_move != null) {
                m_move.moveDir = value;
            }
            else {
                Debug.LogWarning("ProjectileController: Attempting to set moveDir but no ProjectileMove component found.");
            }
        }
    }
    [HideInInspector] public float moveSpeed
    {
        get => m_move != null ? m_move.moveSpeed : 0f;
        set
        {
            if (m_move != null) {
                m_move.moveSpeed = value;
            }
            else {
                Debug.LogWarning("ProjectileController: Attempting to set moveSpeed but no ProjectileMove component found.");
            }
        }
    }
    [HideInInspector] public float rotationSpeed
    {
        get => m_move != null ? m_move.rotationSpeed : 0f;
        set
        {
            if (m_move != null) {
                m_move.rotationSpeed = value;
            }
            else {
                Debug.LogWarning("ProjectileController: Attempting to set rotationSpeed but no ProjectileMove component found.");
            }
        }
    }
    [HideInInspector] public float arcHeight
    {
        get => m_move is ThrowMove throwMove ? throwMove.arcHeight : 0f;
        set
        {
            if (m_move is ThrowMove throwMove) {
                throwMove.arcHeight = value;
            }
            else {
                Debug.LogWarning("ProjectileController: Attempting to set arcHeight but ProjectileMove component is not a ThrowMove.");
            }
        }
    }
    [HideInInspector] public Vector2? target
    {
        get => m_move != null ? m_move.target : null;
        set
        {
            if (m_move != null) {
                m_move.target = value;
            }
            else {
                Debug.LogWarning("ProjectileController: Attempting to set target but no ProjectileMove component found.");
            }
        }
    }

    [SerializeField] protected float expireTime = 5f;
    [SerializeField] protected AnimationClip m_idleClip;
    [SerializeField] protected AnimationClip m_endClip;
    [SerializeField] protected float m_gizmoRadius = 0.1f;

    protected ProjectileMove m_move { get; private set; }
    protected Animator m_animator { get; private set; }
    protected ProjectileEndAnimSMB m_projectileEndSMB { get; private set; }

    private AnimatorOverrideController m_animOverrideController;
    private string m_idleAnimClipName = "Projectile-Idle";
    private string m_endAnimClipName = "Projectile-End";
    private string m_endTriggerParamName = "End";


    // --- Events ---
    public event Action OnProjectileExpired;


    // --- Methods ---
    // NOTE: This method should be called right after instatiating a new m_projectile.
    // Due to the nature of the member moveDir (a reference to the m_move member), if
    // the creator of the m_projectile wants to set the moveDir immediately after 
    // instantiate the m_projectile (i.e. most likely scenario), the m_move member
    // must be initialized before moveDir is set from the outside.
    public void Init(ActorController thrower, float damage, float moveSpeed, float rotationSpeed)
    {
        m_move = GetComponent<ProjectileMove>();

        this.thrower = thrower;
        this.damage = damage;
        if (m_move != null) {
            m_move.moveSpeed = moveSpeed;
            m_move.rotationSpeed = rotationSpeed;
        }

        SubscribeOnTargetReached();
    }

    protected virtual void Awake()
    {
        // NOTE: Projectile may not have a movement component. Weird but possible.
        m_move = GetComponent<ProjectileMove>();

        m_animator = GetComponent<Animator>();
        if (m_animator != null) {
            m_animOverrideController = new AnimatorOverrideController(m_animator.runtimeAnimatorController);
            m_animator.runtimeAnimatorController = m_animOverrideController;
        }
    }

    // NOTE: The handler of the OnExit event will Destroy the gameObject.
    // Extenders of this class should call base.OnEnable() at the end of their
    // OnEnable() method to ensure their handlers run before this handler
    // finishes the m_projectile.
    protected virtual void OnEnable()
    {
        // Susbcribe to the StateExistSMB OnDeadAnimEnd event to know when the explosion animation finishes
        // NOTE: Assuming just one StateExistSMB.
        if (m_animator != null) {
            m_projectileEndSMB = m_animator.GetBehaviour<ProjectileEndAnimSMB>();

            if (m_projectileEndSMB != null) {
                m_projectileEndSMB.OnExit += DestroyProjectile;
            }            
        }

        SubscribeOnTargetReached();
    }

    protected virtual void OnDisable()
    {
        // Unsubscribe from the StateExistSMB OnDeadAnimEnd event to avoid memory leaks
        if (m_projectileEndSMB != null) {
            m_projectileEndSMB.OnExit -= DestroyProjectile;
            m_projectileEndSMB = null;
        }

        UnsubscribeOnTargetReached();
    }

    protected virtual void Start()
    {
        // Set the m_projectile into the same layer as the thrower.
        // We've configured the collision matrix so that Player and Enemy layers
        // don't collide with themselves, so enemy projectiles can't collide with
        // enemiesInScene and enemyPlayer projectiles can't collide with the enemyPlayer.
        if (thrower != null) {
            gameObject.layer = thrower.gameObject.layer;
        }
        else {
            Debug.LogError("Projectile has no thrower assigned in Start!");
        }

        // Set idle animation clip if we have an animator and an idle clip
        if (m_animator != null && m_idleClip != null) {
            if (!GameManager.UpdateAnimClip(m_animOverrideController, m_idleAnimClipName, m_idleClip)) {
                Debug.LogWarning($"Failed to set idle animation clip for {gameObject.name}. Make sure the animator has a state named '{m_idleAnimClipName}' with an AnimationClip assigned.");
            }
        }

        // Set end animation clip if we have an animator and an end clip
        if (m_animator != null && m_endClip != null) {
            if (!GameManager.UpdateAnimClip(m_animOverrideController, m_endAnimClipName, m_endClip)) {
                Debug.LogWarning($"Failed to set end animation clip for {gameObject.name}. Make sure the animator has a state named '{m_endAnimClipName}' with an AnimationClip assigned.");
            }
        }

        StartCoroutine(DestroyAfterTime(expireTime));
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        // NOTE: When a projectile expires we are not triggering any ending effect.
        // We just destroy the projectile.
        OnProjectileExpired?.Invoke();
        Destroy(gameObject);
    }

    private void End()
    {
        // Stop moving immediately.
        moveDir = Vector2.zero;

        // Trigger end effect if any.
        if (m_animator != null) {
            m_animator.SetTrigger(m_endTriggerParamName);

            // NOTE: Here we set the collider to trigger so it can play its end
            // animation and may change the collider size without colliding with
            // other objects (and displacing them) while still being able to check
            // for overlaps with other objects.
            Collider2D collider = GetComponent<Collider2D>();
            if (collider != null) {
                collider.isTrigger = true;
            }
        }
        else {
            DestroyProjectile();
        }

        // We are subscribed to the OnExit event of the End State of the AnimatorController.
    }

    private void SubscribeOnTargetReached() {
        if (m_move != null) {
            m_move.OnTargetReached += End;
        }
    }

    private void UnsubscribeOnTargetReached() {
        if (m_move != null) {
            m_move.OnTargetReached -= End;
        }
    }


    // --- Event Handlers ---
    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }


    // --- Collision Handling ---
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if we are colliding against another projectile.
        // For now we don't want projectiles to interact with each other.
        ProjectileController otherProjectile = collision.gameObject.GetComponent<ProjectileController>();
        if (otherProjectile != null) {

            // BUG: Projectiles colliding in opposite directions can get stuck.
            // Effectively pushing each other and not move, or slowly in the
            // perpendicular direction.

            return;
        }

        // Check if the collided object is an actor.
        ActorController hitActor = collision.gameObject.GetComponent<ActorController>();
        if (hitActor != null && thrower != null) {
            bool throwerIsPlayer = thrower.CompareTag("Player");
            bool hitIsPlayer = hitActor.CompareTag("Player");
            bool throwerIsEnemy = thrower.CompareTag("Enemy");
            bool hitIsEnemy = hitActor.CompareTag("Enemy");

            // Player projectiles should not hit the enemyPlayer, only enemiesInScene
            if (throwerIsPlayer && hitIsPlayer) {
                // Ignore collision with self
                return;
            }

            // Enemy projectiles should not hit other enemiesInScene
            if (throwerIsEnemy && hitIsEnemy) {
                // Ignore collision with other enemiesInScene
                return;
            }

            // Apply damage if valid target
            HPStat hp = collision.gameObject.GetComponent<HPStat>();
            if (hp != null) {
                hp.TakeDamage(damage);
            }
        }

        // Here we know we hit something different than a projectile.
        // End the projectile.
        End();
    }
}
