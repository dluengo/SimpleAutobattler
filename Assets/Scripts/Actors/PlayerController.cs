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

    private void Update()
    {
        //NOTE: We have to check every frame for the attack button being held down.
        // This is to allow for changing the attack target while holding more
        // than one button.
        if (actor.actions.Count > 0 && actor.actions[0].keepCasting) {
            Vector2 attackDirection = PollAttackDirection();

            // StartActionDirection updates the attack target even if the action is
            // being performed.
            actor.actions[0].StartActionDirection(attackDirection);
        }
    }

    // --- Helper Methods ---
    private Vector2 GetAttackDirection(InputAction.CallbackContext context)
    {
        // NOTE: This is a terrible design, but it works for now.
        //var control = context.control;
        var activeControl = context.action.activeControl;

        if (activeControl != null) {
            switch (activeControl.name) {
                case "upArrow":
                case "buttonNorth":
                    return Vector2.up;

                case "downArrow":
                case "buttonSouth":
                    return Vector2.down;

                case "leftArrow":
                case "buttonWest":
                    return Vector2.left;

                case "rightArrow":
                case "buttonEast":
                    return Vector2.right;
            }
        }

        // Fallback to movement target or right
        if (actor.move != null && actor.move.moveDir != Vector2.zero) {
            return actor.move.moveDir;
        }

        return Vector2.right;
    }

    private Vector2 PollAttackDirection()
    {
        // Keyboard arrows
        if (Keyboard.current != null) {
            if (Keyboard.current.upArrowKey.isPressed) return Vector2.up;
            if (Keyboard.current.downArrowKey.isPressed) return Vector2.down;
            if (Keyboard.current.leftArrowKey.isPressed) return Vector2.left;
            if (Keyboard.current.rightArrowKey.isPressed) return Vector2.right;
        }

        if (Gamepad.current != null) {
            // Gamepad right stick
            Vector2 rightStick = Gamepad.current.rightStick.ReadValue();
            if (rightStick.magnitude > m_minInputThreshold) {
                return rightStick.normalized;
            }

            // Gamepad face buttons
            if (Gamepad.current.buttonNorth.isPressed) return Vector2.up;
            if (Gamepad.current.buttonSouth.isPressed) return Vector2.down;
            if (Gamepad.current.buttonWest.isPressed) return Vector2.left;
            if (Gamepad.current.buttonEast.isPressed) return Vector2.right;
        }

        // Fallback to movement target or right
        if (actor.move != null && actor.move.moveDir != Vector2.zero)
            return actor.move.moveDir;

        return Vector2.right;
    }


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
        if (actor.actions.Count == 0) {
            return;
        }

        if (context.performed) {
            Vector2 attackDirection = GetAttackDirection(context);
            actor.actions[0].keepCasting = true;
            actor.actions[0].StartActionDirection(attackDirection);
        }
        else if (context.canceled) {
            actor.actions[0].keepCasting = false;
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
