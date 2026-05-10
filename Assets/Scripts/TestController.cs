using UnityEngine;
using UnityEngine.InputSystem;

public class TestController : MonoBehaviour
{
    [SerializeField] string m_deadParamName = "isDead";
    private Animator m_animator;


    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        Debug.Assert(m_animator != null, "TestController requires an Animator component.");
    }

    private void OnEnable()
    {
        // Susbcribe to DeadAnimSMB.OnDeadAnimEnd
        if (m_animator != null) {
            foreach (var behaviour in m_animator.GetBehaviours<DeadAnimSMB>()) {
                behaviour.OnDeadAnimEnd += DeadAnimEndHandler;
            }
        }
    }

    // Handler for when the death animation ends
    private void DeadAnimEndHandler()
    {
        Debug.Log("Death animation ended.");
        Destroy(gameObject);
    }

    // Input handler for testing purposes
    public void OnActionInput(InputAction.CallbackContext context)
    {
        if (context.performed) {
            m_animator.SetBool(m_deadParamName, true);
        }
    }
}
