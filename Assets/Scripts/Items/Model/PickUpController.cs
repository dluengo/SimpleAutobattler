using UnityEngine;
using System;

// NOTE: A trigger is needed for this class to work.
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class PickUpController : MonoBehaviour
{
    // --- Members ---
    [Header("--- PickUp Settings ---")]
    [SerializeField] protected ItemSO m_itemSO;

    protected Animator m_animator;
    protected AnimatorOverrideController m_animOverrideController;
    protected string m_animTriggerParamName = "PickedUp";
    protected string m_idleClipName = "PickUp-Idle";
    protected string m_pickedUpClipName = "PickUp-PickedUp";

    private Item m_item;
    public Item item
    {
        get => m_item;
        protected set {
            if (m_item != value) {
                m_item = value;
                ItemSetup(m_item);
            }
        }
    }

    // NOTE: This bool is used to allow external modules to tell
    // the PickUpController that it has been picked up.
    private bool m_isPickedUp = false;
    [HideInInspector] public bool isPickedUp
    {
        get => m_isPickedUp;
        set
        {
            m_isPickedUp = value;

            // m_item has been picked up.
            if (m_isPickedUp) {
                // Trigger event
                OnPickedUp?.Invoke();

                // Trigger the picked up animation.
                m_animator.SetTrigger(m_animTriggerParamName);

                // NOTE: We are subscribed to the OnPickUpAnimEnd event of the PickUpAnimSMB.
                // When the animation finishes, the EndPickUp() method will be called.
            }
        }
    }

    private PickUpAnimSMB m_pickUpAnimSMB;


    // --- Events ---
    public event Action OnPickedUp;


    // --- Methods ---
    // NOTE: To be called right after Instantiate()
    public void Init(ItemSO itemSO)
    { 
        m_itemSO = itemSO;
        item = m_itemSO.CreateNewItem();
    }

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

        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null && !collider.isTrigger) {
            Debug.LogWarning($"PickUpController on {gameObject.name} needs a Trigger to work properly.");
        }
    }

    protected virtual void OnEnable()
    {
        if (m_pickUpAnimSMB != null) {
            m_pickUpAnimSMB.OnPickUpAnimEnd += EndPickUp;
        }
    }

    protected virtual void OnDisable()
    {
        if (m_pickUpAnimSMB != null) {
            m_pickUpAnimSMB.OnPickUpAnimEnd -= EndPickUp;
        }
    }

    protected virtual void Start()
    {
        // Safety check.
        if (m_itemSO == null) {
            Debug.LogWarning($"PickUpController on {gameObject.name} does not have an ItemSO assigned.");
            return;
        }

        if (item == null) {
            Init(m_itemSO);
        }
    }


    // --- Private Methods ---
    private void EndPickUp()
    {
        Destroy(gameObject);
    }

    private void ItemSetup(Item item)
    {
        if (item != null) {
            // Set the sprite of the pickup to the icon of the item.
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && item != null) {
                spriteRenderer.sprite = item.icon;
            }
        }

        // Change the idle animation if we have been provided with a clip.
        //if (m_idleClip != null && m_animOverrideController != null) {
        if (item != null && item.idleClip != null && m_animOverrideController != null) {
            if (!GameManager.UpdateAnimClip(m_animOverrideController, m_idleClipName, item.idleClip)) {
                Debug.LogWarning($"Failed to update idle animation clip for {gameObject.name}.");
            }
        }

        // Change the picked up animation clip if we have been provided with a clip.
        //if (m_pickedUpClip != null && m_animOverrideController != null) {
        if (item != null && item.pickedUpClip != null && m_animOverrideController != null) {
            if (!GameManager.UpdateAnimClip(m_animOverrideController, m_pickedUpClipName, item.pickedUpClip)) {
                Debug.LogWarning($"Failed to update picked up animation clip for {gameObject.name}.");
            }
        }
    }


    // --- Collision Handling ---
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        // NOTE: We do it this way, pickup signaling actor they are passing
        // through us because we want pickups to be Collision Triggers so they
        // don't physically interfere with actors. Because of how Triggers work
        // we need it to do it this way.

        // Check if an actor is passing through the pick up.
        ActorController actor = collision.gameObject.GetComponent<ActorController>();
        if (actor != null) {
            // Tell the actor they should try to pick up if they can.
            actor.PickUp(this);
        }
    }
}
