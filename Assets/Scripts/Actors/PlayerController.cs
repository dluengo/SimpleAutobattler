using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ActorController))]
public class PlayerController : MonoBehaviour
{
    // --- Members ---
    public ActorController actor { get; private set; }

    [SerializeField] float m_minInputThreshold = 0.1f;


    // --- Methods ---
    private void Awake()
    {
        actor = GetComponent<ActorController>();
        Debug.Assert(actor != null, "PlayerController requires an ActorController component.");
    }

    ////protected override void OnAttackPerformedEventHandler()
    ////{
    ////    if (m_projectilePrefab != null && projectileSpawnPoint != null) {
    ////        // Shoot in the moveDir the player is looking.

    ////        // NOTE: This shouldn't occur, but control it anyway.
    ////        Vector2 shootDirection = m_attackDir;
    ////        if (shootDirection == Vector2.zero) {
    ////            shootDirection = Vector2.right;
    ////        }

    ////        // Instantiate the projectile with the calculated rotation
    ////        GameObject projectileGO = Instantiate(
    ////            m_projectilePrefab,
    ////            projectileSpawnPoint.position,
    ////            Quaternion.identity
    ////        );

    ////        ProjectileController projectile = projectileGO.GetComponent<ProjectileController>();
    ////        if (projectile != null) {
    ////            projectile.direction = shootDirection.normalized;
    ////            projectile.thrower = this;
    ////        }
    ////    }
    ////}

    //protected void UpdateAttackDirectionFromInput()
    //{
    //    Vector2? attackDirection = null;

    //    // Keyboard arrows
    //    if (Keyboard.current != null) {
    //        if (Keyboard.current.upArrowKey.isPressed) {
    //            attackDirection = Vector2.up;
    //        } else if (Keyboard.current.downArrowKey.isPressed) {
    //            attackDirection = Vector2.down;
    //        } else if (Keyboard.current.leftArrowKey.isPressed) {
    //            attackDirection = Vector2.left;
    //        } else if (Keyboard.current.rightArrowKey.isPressed) {
    //            attackDirection = Vector2.right;
    //        }
    //    }

    //    // Gamepad face buttons
    //    if (attackDirection == null && Gamepad.current != null) {
    //        if (Gamepad.current.buttonNorth.isPressed) {
    //            attackDirection = Vector2.up;
    //        } else if (Gamepad.current.buttonSouth.isPressed) {
    //            attackDirection = Vector2.down;
    //        } else if (Gamepad.current.buttonWest.isPressed) {
    //            attackDirection = Vector2.left;
    //        } else if (Gamepad.current.buttonEast.isPressed) {
    //            attackDirection = Vector2.right;
    //        }
    //    }
    //}


    // --- Input System Handlers ---
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.performed) {
            Vector2 input = context.ReadValue<Vector2>();
            if (input.magnitude < m_minInputThreshold) {
                actor.move.moveDir = Vector2.zero;
            }
            else {
                actor.move.moveDir = input.normalized;
            }
        }
        else {
            actor.move.moveDir = Vector2.zero;
        }
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.performed) {
            if (actor.actions.Count > 0) {
                actor.actions[0].keepGoing = true;
                actor.actions[0].StartAction();
            }
        }
        else if (context.canceled) {
            if (actor.actions.Count > 0) {
                actor.actions[0].keepGoing = false;
            }
        }
    }

    // NOTE: The design is flawed. There should be an InputManager that handles
    // all input and then calls methods on the PlayerController.
    // This is a quick and dirty solution to trigger an action in the GameManager
    // when the space key is pressed.
    public void OnTriggerAction(InputAction.CallbackContext context)
    {
        if (context.performed) {
            GameManager.Instance.TriggerAction();
        }
    }
}
