using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : ActorController
{
    // --- Members ---
    //public bool isAttacking { get; private set; } = false;

    //[SerializeField] float moveSpeed = 5f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawnPoint;
    [SerializeField] float m_minInputThreshold = 0.1f;

    //private Vector3 m_look;
    //private Vector3 m_direction;
    //private Rigidbody2D m_rb;
    //private Animator m_animator;
    //private AttackEndSMB m_attackEndSMB;
    //private bool m_attackInput = false;

    //// --- Methods ---
    //private void Awake()
    //{
    //    m_rb = GetComponent<Rigidbody2D>();
    //    Debug.Assert(m_rb != null, "PlayerController: Rigidbody2D component is missing.");

    //    m_animator = GetComponent<Animator>();
    //    Debug.Assert(m_animator != null, "PlayerController: Animator component is missing.");
    //}

    //private void OnEnable()
    //{
    //    // Subscribe to AttackEndSMB.OnAttackEnd
    //    if (m_animator != null) {
    //        foreach (var behaviour in m_animator.GetBehaviours<AttackEndSMB>()) {
    //            m_attackEndSMB = behaviour;
    //            m_attackEndSMB.OnAttackEnd += HandleAttackEnd;
    //        }
    //    }
    //}

    //private void OnDisable()
    //{
    //    // Unsubscribe from AttackEndSMB.OnAttackEnd
    //    if (m_attackEndSMB != null) {
    //        m_attackEndSMB.OnAttackEnd -= HandleAttackEnd;
    //    }
    //}

    //private void FixedUpdate()
    //{
    //    //transform.position += m_direction.normalized * moveSpeed * Time.deltaTime;
    //    m_rb.MovePosition(transform.position + m_direction.normalized * moveSpeed * Time.fixedDeltaTime);
    //}

    //private bool IsFlipNeeded()
    //{
    //    if (m_direction.x > 0f && transform.localScale.x < 0f) {
    //        return true;
    //    }
    //    else if (m_direction.x < 0f && transform.localScale.x > 0f) {
    //        return true;
    //    }

    //    return false;
    //}

    //private void Flip()
    //{
    //    transform.localScale = new Vector3(
    //        -transform.localScale.x,
    //        transform.localScale.y,
    //        transform.localScale.z);
    //}

    //private void StartAttack()
    //{
    //    isAttacking = true;
    //    m_animator.SetBool("isAttacking", true);
    //}

    //private void HandleAttackEnd()
    //{
    //    if (!m_attackInput) { 
    //        isAttacking = false;
    //        m_animator.SetBool("isAttacking", false);
    //    }
    //}

    protected override void OnAttackPerformedEventHandler()
    {
        if (projectilePrefab != null && projectileSpawnPoint != null) {
            // Only shoot if there is a direction
            Vector2 shootDirection = m_look;
            if (shootDirection == Vector2.zero) {
                // Default to right if not moving
                shootDirection = Vector2.right;
            }

            // Instantiate the projectile with the calculated rotation
            GameObject projectileGO = Instantiate(
                projectilePrefab,
                projectileSpawnPoint.position,
                Quaternion.identity
            );

            ProjectileController projectile = projectileGO.GetComponent<ProjectileController>();
            if (projectile != null) {
                projectile.direction = shootDirection.normalized;
            }
        }
    }


    // --- Input System Handlers ---
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        //Debug.Log($"Move input: {input}, magnitude: {input.magnitude}");
        if (input.magnitude < m_minInputThreshold) {
            //Debug.Log("Input below threshold, treating as zero.");
            direction = Vector2.zero;
        }
        else {
            //Debug.Log("Input above threshold, processing movement.");
            m_look = input.normalized;
            direction = input.normalized;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed) {
            m_attackInput = true;
            if (!isAttacking) {
                StartAttack();
            }
        }
        else if (context.canceled) {
            m_attackInput = false;
        }
    }
}
