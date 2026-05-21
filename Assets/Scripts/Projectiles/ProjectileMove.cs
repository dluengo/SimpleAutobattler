using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(ProjectileController))]
public class ProjectileMove : MonoBehaviour
{
    // --- Members ---
    [HideInInspector] public Vector2 moveDir = Vector2.zero;
    [HideInInspector] public Vector2? target = null;

    [Header("--- Projectile Movement Settings ---")]
    public float moveSpeed = 10f;
    public float rotationSpeed = 0f;
    public bool enableDirGizmo = false;

    protected Rigidbody2D m_rb;
    protected ProjectileController m_projectile;


    // --- Events ---
    public event Action OnTargetReached;


    // --- Methods ---
    protected virtual void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();

        m_projectile = GetComponent<ProjectileController>();
        Debug.Assert(m_projectile != null, "ProjectileMove: ProjectileController component is missing.");
    }

    protected virtual void Start()
    {
        // Set the initial moveDir towards the target if it exists.
        if (target.HasValue) {
            moveDir = target.Value - (Vector2)transform.position;
        }

        // Initial rotation to face the moveDir
        if (moveDir != Vector2.zero) {
            float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    protected virtual void Update()
    {
        // Check if we reached the target (if we have one).
        // NOTE: This will keep triggering as long as we are at the target.
        if (target.HasValue) {
            float distanceToTarget = Vector2.Distance(transform.position, target.Value);
            if (distanceToTarget <= 0) {
                OnTargetReachedInvoke();
            }
        }
    }

    private void FixedUpdate()
    {
        // Projectiles moves in two ways:
        // 1. If there is a target:
        //    a. If we are close enough to the target, snap to it and trigger OnTargetReached.
        //    b. If we are still far from the target, keep moving in the moveDir direction.
        //       This direction should be set by extenders of the class (e.g. ShotMove).
        //
        // 2. If there is no target but there is a moveDir, it keeps moving in that direction.

        if (target.HasValue) {
            Vector2 currentPosition = transform.position;
            Vector2 toTarget = target.Value - currentPosition;
            float distanceToTarget = toTarget.magnitude;
            float step = moveSpeed * Time.fixedDeltaTime;

            // Close enought to the target, snap to it.
            if (distanceToTarget <= step) {
                transform.position = target.Value;
                moveDir = Vector2.zero;
                OnTargetReachedInvoke();
            }
            // Target still far, move using moveDir.
            else {
                MoveAndRotate();
            }
        }
        // No target, move in the moveDir direction.
        else {
            MoveAndRotate();
        }
    }

    protected void OnTargetReachedInvoke()
    {
        OnTargetReached?.Invoke();
    }

    private void MoveAndRotate()
    {
        // NOTE: Projectiles are allowed to have no rigidbody, but is highly desirable.

        // There's a rigidbody, use MovePosition for better physics interactions.
        if (m_rb != null) {
            Vector2 newPos = (Vector2)transform.position + moveDir.normalized * moveSpeed * Time.fixedDeltaTime;
            m_rb.MovePosition(newPos);
        }
        // No rigidbody, just move the transform.
        else {
            transform.Translate(moveDir.normalized * moveSpeed * Time.fixedDeltaTime, Space.World);
        }

        // Make the projectile rotate if there is rotationSpeed.
        if (rotationSpeed != 0f) {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.fixedDeltaTime);
        }
    }


    // --- Gizmos ---
    private void OnDrawGizmos()
    {
        if (enableDirGizmo) {
            // Draw a red line in the direction of moveDir for debugging
            if (m_projectile != null && target.HasValue) {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, target.Value);
            }
        }
    }
}
