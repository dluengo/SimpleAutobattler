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
            // Shoot in the m_moveDir the player is looking.

            // NOTE: This shouldn't occur, but control it anyway.
            Vector2 shootDirection = m_attackDir;
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

    protected void UpdateAttackDirectionFromInput()
    {
        Vector2? attackDirection = null;

        // Keyboard arrows
        if (Keyboard.current != null) {
            if (Keyboard.current.upArrowKey.isPressed) {
                attackDirection = Vector2.up;
            } else if (Keyboard.current.downArrowKey.isPressed) {
                attackDirection = Vector2.down;
            } else if (Keyboard.current.leftArrowKey.isPressed) {
                attackDirection = Vector2.left;
            } else if (Keyboard.current.rightArrowKey.isPressed) {
                attackDirection = Vector2.right;
            }
        }

        // Gamepad face buttons
        if (attackDirection == null && Gamepad.current != null) {
            if (Gamepad.current.buttonNorth.isPressed) {
                attackDirection = Vector2.up;
            } else if (Gamepad.current.buttonSouth.isPressed) {
                attackDirection = Vector2.down;
            } else if (Gamepad.current.buttonWest.isPressed) {
                attackDirection = Vector2.left;
            } else if (Gamepad.current.buttonEast.isPressed) {
                attackDirection = Vector2.right;
            }
        }

        m_attackDir = attackDirection ?? m_look;
    }


    // --- Input System Handlers ---
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        if (input.magnitude < m_minInputThreshold) {
            m_moveDir = Vector2.zero;
        } else {
            m_look = input.normalized;
            m_moveDir = input.normalized;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed) {
            m_attackInput = true;

            UpdateAttackDirectionFromInput();

            //Debug.Log($"Attack input direction: {m_attackDir}");
            StartAttack();
        }
        else if (context.canceled) {
            //Debug.Log("Attack input canceled.");
            m_attackInput = false;
        }
    }
}
