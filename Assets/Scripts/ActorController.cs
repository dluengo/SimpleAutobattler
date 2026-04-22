using UnityEngine;

public abstract class ActorController : MonoBehaviour
{
    // --- Members ---
    public bool isAttacking { get; private set; } = false;

    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] string m_animAttackParamName = "isAttacking";
    [SerializeField] string m_animMoveParamName = "isMoving";

    protected Vector2 m_look = Vector2.right;
    private Vector2 m_direction;
    protected Vector2 direction {
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

    private AttackEndSMB m_attackEndSMB;


    // --- Methods ---
    protected virtual void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        Debug.Assert(m_rb != null, "ActorController: Rigidbody2D component is missing.");

        m_animator = GetComponent<Animator>();
        Debug.Assert(m_animator != null, "ActorController: Animator component is missing.");
    }

    protected virtual void OnEnable()
    {
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
        m_rb.MovePosition((Vector2)transform.position + m_direction.normalized * moveSpeed * Time.fixedDeltaTime);
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
        transform.localScale = new Vector3(
            -transform.localScale.x,
            transform.localScale.y,
            transform.localScale.z);
    }

    protected void StartAttack()
    {
        isAttacking = true;
        m_animator.SetBool(m_animAttackParamName, true);
    }

    private void HandleAttackEnd()
    {
        if (!m_attackInput) {
            isAttacking = false;
            m_animator.SetBool(m_animAttackParamName, false);
        }
    }

    protected virtual void OnAttackPerformedEventHandler()
    {
        Debug.Log("ActorController: OnAttackPerformedEventHandler called.");
    }
}
