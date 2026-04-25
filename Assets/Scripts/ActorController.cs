using System.Collections;
using UnityEngine;

public abstract class ActorController : MonoBehaviour
{
    // --- Members ---
    private bool m_isAttacking = false;
    public bool isAttacking {
        get => m_isAttacking;
        private set {
            m_isAttacking = value;
            if (m_isAttacking) {
                m_attackAnimRunning = true;
                m_animator.SetBool(m_animAttackParamName, true);
            }
            else {
                m_attackAnimRunning = false;
                m_animator.SetBool(m_animAttackParamName, false);
            }
        }
    }
    [SerializeField] LayerMask m_enemyLayer;
    public LayerMask enemyLayer
    {
        get => m_enemyLayer;
        private set => m_enemyLayer = value;
    }

    [SerializeField] protected float m_moveSpeed = 5f;
    [SerializeField] protected float m_attackSpeed = 1f;
    [SerializeField] string m_animAttackParamName = "isAttacking";
    [SerializeField] string m_animMoveParamName = "isMoving";
    [SerializeField] string m_attackAnimClipName = "Archer-Attack";
    [SerializeField] string m_attackSpeedMultiplierParamName = "AttackSpeedMultiplier";

    protected Vector2 m_look = Vector2.right;
    private Vector2 m_direction;
    protected Vector2 m_moveDir {
        get => m_direction;
        set {
            m_direction = value;

            if (IsFlipNeeded()) {
                Flip();
            }

            if (m_direction != Vector2.zero) {
                m_animator.SetBool(m_animMoveParamName, true);
            } else {
                m_animator.SetBool(m_animMoveParamName, false);
            }
        }
    }
    protected Rigidbody2D m_rb;
    protected Animator m_animator;
    protected bool m_attackInput = false;
    protected bool m_attackOnCooldown = false;
    protected Vector2 m_attackDir;

    private AttackEndSMB m_attackEndSMB;
    private float m_attackAnimDuration;
    private bool m_attackAnimRunning = false;
    private bool m_allowFlip = true;


    // --- Methods ---
    protected virtual void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        Debug.Assert(m_rb != null, "ActorController: Rigidbody2D component is missing.");

        m_animator = GetComponent<Animator>();
        Debug.Assert(m_animator != null, "ActorController: Animator component is missing.");

        // Get the duration of the attack animation clip
        m_attackAnimDuration = -1f;
        RuntimeAnimatorController controller = m_animator.runtimeAnimatorController;
        if (controller != null) {
            foreach (var clip in controller.animationClips) {
                if (clip.name == m_attackAnimClipName) {
                    m_attackAnimDuration = clip.length;
                    break;
                }
            }
        }
    }

    protected virtual void OnEnable()
    {
        enemyLayer = m_enemyLayer;

        // Subscribe to AttackEndSMB.OnAttackEnd
        if (m_animator != null) {
            foreach (var behaviour in m_animator.GetBehaviours<AttackEndSMB>()) {
                m_attackEndSMB = behaviour;
                m_attackEndSMB.OnAttackEnd += HandleAttackEnd;
            }
        }
    }

    protected virtual void OnDisable()
    {
        // Unsubscribe from AttackEndSMB.OnAttackEnd
        if (m_attackEndSMB != null) {
            m_attackEndSMB.OnAttackEnd -= HandleAttackEnd;
        }
    }

    protected virtual void FixedUpdate()
    {
        m_rb.MovePosition((Vector2)transform.position + m_direction.normalized * m_moveSpeed * Time.fixedDeltaTime);
    }

    private bool IsFlipNeeded()
    {
        if (m_direction.x > 0f && transform.localScale.x < 0f) {
            return true;
        }
        else if (m_direction.x < 0f && transform.localScale.x > 0f) {
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

            m_look = -m_look;
        }
    }

    protected void StartAttack()
    {
        if (m_attackOnCooldown || m_attackAnimRunning) {
            return;
        }

        isAttacking = true;
        m_attackOnCooldown = true;

        //Debug.Log($"Attack started with m_moveDir {m_attackDir} and look {m_look}.");
        // Flip to face the attack m_moveDir if needed
        if (m_attackDir.x < 0f && m_look.x > 0f ||
            m_attackDir.x > 0f && m_look.x < 0f) {
            //Debug.Log("Flipping to face attack m_moveDir.");
            Flip();
        }

        m_allowFlip = false;

        // Adjust animation projectileSpeed if needed
        float cooldown = 1f / m_attackSpeed;
        float speedMultiplier = m_attackAnimDuration > cooldown ? m_attackAnimDuration / cooldown : 1f;
        m_animator.SetFloat(m_attackSpeedMultiplierParamName, speedMultiplier);

        StartCoroutine(AttackCooldownCR());
    }

    private IEnumerator AttackCooldownCR()
    {
        //Debug.Log($"Attack started, entering cooldown of {1f / m_attackSpeed} seconds.");

        //BUG: If the duration of the animation is longer than the cooldown,
        // it makes the attack rate irregular.
        yield return new WaitForSeconds(1f / m_attackSpeed);
        //Debug.Log("Attack cooldown ended.");
        m_attackOnCooldown = false;

        // If there is still input after the cooldown, start the next attack immediately
        if (m_attackInput) {
            //Debug.Log("Attack input received during cooldown, starting next attack.");
            StartAttack();
        }
    }

    private void HandleAttackEnd()
    {
        m_attackAnimRunning = false;
        m_allowFlip = true;

        // Flip if needed
        if (m_moveDir.x > 0f && transform.localScale.x < 0f ||
            m_moveDir.x < 0f && transform.localScale.x > 0f) {
            Flip();
        }

        //Debug.Log("Attack animation ended.");
        isAttacking = false;

        // If there is still input after the attack animation ends, start the next attack immediately
        if (m_attackInput) {
            //Debug.Log("More attack input, starting next attack.");
            StartAttack();
        }
    }

    protected virtual void OnAttackPerformedEventHandler()
    {
        Debug.Log("ActorController: OnAttackPerformedEventHandler called.");
    }


    // --- Gizmos ---
    private void OnDrawGizmosSelected()
    {
        // Draw a line indicating the look m_moveDir
        Gizmos.color = Color.blue;
        Vector3 lookDirection = new Vector3(m_look.x, m_look.y, 0f);
        Gizmos.DrawLine(transform.position, transform.position + lookDirection);
    }
}
