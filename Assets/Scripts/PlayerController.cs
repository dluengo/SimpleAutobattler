using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : ActorController
{
    // --- Members ---
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawnPoint;
    [SerializeField] float m_minInputThreshold = 0.1f;


    // --- Methods ---
    protected override void OnAttackPerformedEventHandler()
    {
        if (projectilePrefab != null && projectileSpawnPoint != null) {
            // Shoot in the direction the player is looking.

            // NOTE: This shouldn't occur, but control it anyway.
            Vector2 shootDirection = m_look;
            if (shootDirection == Vector2.zero) {
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
