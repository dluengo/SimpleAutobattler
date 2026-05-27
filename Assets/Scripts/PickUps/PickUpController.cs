using UnityEngine;
using System;

// NOTE: A trigger is needed for this class to work.
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class PickUpController : MonoBehaviour
{
    // --- Members ---
    [Header("--- PickUp Settings ---")]
    [SerializeField] protected AnimationClip m_idleClip;
    [SerializeField] protected AnimationClip m_pickedUpClip;

    protected Animator m_animator;
    protected AnimatorOverrideController m_animOverrideController;
    protected string m_animTriggerParamName = "PickedUp";
    protected string m_idleClipName = "PickUp-Idle";
    protected string m_pickedUpClipName = "PickUp-PickedUp";

    private PickUpAnimSMB m_pickUpAnimSMB;


    // --- Events ---
    public event Action OnPickedUp;
    public event Action OnDestroy;


    // --- Methods ---
    protected virtual void Awake()
    {
        m_animator = GetComponent<Animator>();
        Debug.Assert(m_animator != null, "PickUpController requires an Animator component.");

        m_pickUpAnimSMB = m_animator.GetBehaviour<PickUpAnimSMB>();
        Debug.Assert(m_pickUpAnimSMB != null, "Animator does not have a PickUpAnimSMB behaviour.");

        // NOTE: AnimationClips are changeable at runtime, so we need to use an
        // AnimatorOverrideController to override the clips in the animator controller.
        m_animOverrideController = new AnimatorOverrideController(m_animator.runtimeAnimatorController);
        if (m_animator != null) {
            m_animator.runtimeAnimatorController = m_animOverrideController;
        }
    }

    protected virtual void OnEnable()
    {
        if (m_pickUpAnimSMB != null) {
            m_pickUpAnimSMB.OnPickUpAnimEnd += End;
        }

        // Change the idle animation if we have been provided with a clip.
        if (m_idleClip != null && m_animOverrideController != null) {
            if (!GameManager.UpdateAnimClip(m_animOverrideController, m_idleClipName, m_idleClip)) {
                Debug.LogWarning($"Failed to update idle animation clip for {gameObject.name}.");
            }
        }

        // Change the picked up animation clip if we have been provided with a clip.
        if (m_pickedUpClip != null && m_animOverrideController != null) {
            if (!GameManager.UpdateAnimClip(m_animOverrideController, m_pickedUpClipName, m_pickedUpClip)) {
                Debug.LogWarning($"Failed to update picked up animation clip for {gameObject.name}.");
            }
        }
    }

    protected virtual void OnDisable()
    {
        if (m_pickUpAnimSMB != null) {
            m_pickUpAnimSMB.OnPickUpAnimEnd -= End;
        }
    }


    // --- Private Methods ---
    // NOTE: Override this for custom pickup restrictions.
    // NOTE: This method is not a simple boolean method. It is supposed to deal
    // with the logic of picking up the item. The result tells if the pickup
    // was successfully picked up or not.
    protected virtual bool TryPickUp(GameObject picker)
    {
        return true;
    }

    private void End()
    {
        OnDestroy?.Invoke();
        Destroy(gameObject);
    }


    // --- Collision Handler ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            if (TryPickUp(other.gameObject)) {
                m_animator.SetTrigger(m_animTriggerParamName);
                OnPickedUp?.Invoke();

                // NOTE: We are subscribed to the OnPickUpAnimEnd event.
                // There is where the pickup will be destroyed, after
                // the pickup animation has finished.
            }
        }
    }
}
