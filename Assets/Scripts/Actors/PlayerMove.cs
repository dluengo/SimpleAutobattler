using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : ActorMove
{
    // --- Members ---
    [Header("--- Player Movement Settings ---")]
    [SerializeField] float m_minInputThreshold = 0.1f;


    // --- Methods ---
    protected override void Update()
    {
        // Just let the OnMove handler update the move direction
    }

    // --- Input System Handlers ---
    public void OnMoveInputHandler(InputAction.CallbackContext context)
    {
        if (enableMovement) {
            if (context.performed) {
                Vector2 input = context.ReadValue<Vector2>();
                if (input.magnitude < m_minInputThreshold) {
                    moveDir = Vector2.zero;
                }
                else {
                    moveDir = input.normalized;
                }
            }
            else if (context.canceled) {
                moveDir = Vector2.zero;
            }
        }
    }
}
