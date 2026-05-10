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

    [HideInInspector]
    public Vector2 target = Vector2.zero;

    [SerializeField] protected float expireTime = 5f;
    [SerializeField] protected AnimationClip m_idleClip;
    [SerializeField] protected AnimationClip m_endClip;
    [SerializeField] protected float m_gizmoRadius = 0.1f;

    protected Rigidbody2D m_rb { get; private set; }
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
    protected virtual void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        //Debug.Assert(m_rb != null, "ProjectileController: Rigidbody2D component is missing.");

        // NOTE: Projectile may not have a movement component. Weird but possible.
        m_move = GetComponent<ProjectileMove>();

        m_animator = GetComponent<Animator>();
        Debug.Assert(m_animator != null, "ProjectileController: Animator component is missing.");
        if (m_animator != null) {
            m_animOverrideController = new AnimatorOverrideController(m_animator.runtimeAnimatorController);
            m_animator.runtimeAnimatorController = m_animOverrideController;
        }
    }

    // NOTE: The handler of the OnExit event will Destroy the gameObject.
    // Extenders of this class should call base.OnEnable() at the end of their
    // OnEnable() method to ensure their handlers run before this handler
    // finishes the projectile.
    protected virtual void OnEnable()
    {
        // Susbcribe to the StateExistSMB OnDeadAnimEnd event to know when the explosion animation finishes
        if (m_animator != null) {
            foreach (var behaviour in m_animator.GetBehaviours<ProjectileEndAnimSMB>()) {
                m_projectileEndSMB = behaviour;
                m_projectileEndSMB.OnExit += DestroyProjectile;
            }
        }
    }

    protected virtual void OnDisable()
    {
        // Unsubscribe from the StateExistSMB OnDeadAnimEnd event to avoid memory leaks
        if (m_projectileEndSMB != null) {
            m_projectileEndSMB.OnExit -= DestroyProjectile;
            m_projectileEndSMB = null;
        }
    }

    protected virtual void Start()
    {
        // Set the projectile into the same layer as the thrower.
        // We've configured the collision matrix so that Player and Enemy layers
        // don't collide with themselves, so enemy projectiles can't collide with
        // enemies and player projectiles can't collide with the player.
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
        OnProjectileExpired?.Invoke();
        Destroy(gameObject);
    }

    protected virtual void Update()
    {
        // Rotate to face move direction if we have a movement component and are moving
        if (m_move != null && m_move.moveDir != Vector2.zero) {
            float angle = Mathf.Atan2(m_move.moveDir.y, m_move.moveDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    private void FixedUpdate()
    {
        if (m_move != null && m_move.moveDir != Vector2.zero) {

            // There's a rigidbody in this projectile.
            if (m_rb != null) {
                m_rb.MovePosition(
                    (Vector2)transform.position + m_move.moveDir.normalized * m_move.projectileSpeed * Time.fixedDeltaTime);
            }
            // No rigidbody, just move the transform.
            else {
                //transform.position += (Vector3)(m_move.moveDir.normalized * m_move.projectileSpeed * Time.fixedDeltaTime);
                transform.Translate(m_move.moveDir.normalized * m_move.projectileSpeed * Time.fixedDeltaTime, Space.World);
            }
        }
    }

    private void End()
    {
        // Trigger end effect if any.
        if (m_animator != null) {
            m_animator.SetTrigger(m_endTriggerParamName);
        }
        else {
            DestroyProjectile();
        }

        // We are subscribed to the OnExit event of the End State of the AnimatorController.
    }


    // --- Event Handlers ---
    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }


    // --- Collision Handling ---
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
         // Check if the collided object is an ActorController
        ActorController hitActor = collision.gameObject.GetComponent<ActorController>();
        if (hitActor == null) {
            End();
            return;
        }

        if (thrower != null) {
            bool throwerIsPlayer = thrower.CompareTag("Player");
            bool hitIsPlayer = hitActor.CompareTag("Player");
            bool throwerIsEnemy = thrower.CompareTag("Enemy");
            bool hitIsEnemy = hitActor.CompareTag("Enemy");

            // Player projectiles should not hit the player, only enemies
            if (throwerIsPlayer && hitIsPlayer) {
                // Ignore collision with self
                return;
            }

            // Enemy projectiles should not hit other enemies
            if (throwerIsEnemy && hitIsEnemy) {
                // Ignore collision with other enemies
                return;
            }

            // Apply damage if valid target
            HPStat hp = collision.gameObject.GetComponent<HPStat>();
            if (hp != null) {
                hp.TakeDamage(damage);
            }
        }

        End();
    }


    // --- Gizmos ---
    private void OnDrawGizmos()
    {
        // Draw a red line in the direction of moveDir for debugging
        if (m_move != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(target, m_gizmoRadius);
        }
    }
}
