using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(ActorController), typeof(Animator))]
public abstract class ActorAction : MonoBehaviour
{
    // --- Members ---
    [Header("--- Action Settings ---")]
    public float actionsPerSecond = 1f;

    // NOTE: keepGoing is used to determine if the action should automatically
    // repeat after it finishes its animation and/or its cooldown.
    [HideInInspector] public bool keepGoing = false;
    [HideInInspector] public bool onCooldown { get; protected set; } = false;

    [SerializeField] protected AnimationClip m_animClip;

    protected string m_actionName;
    protected ActorController m_actor;
    protected Animator m_animator;

    private ActionEndSMB m_actionEndSMB;
    private string m_animClipName = "Actor-Action";
    private string m_animParamName = "Action";
    private float m_attackAnimDuration => m_animClip != null ? m_animClip.length : 0f;
    private string m_speedMultiplierParamName = "ActionSpeedMultiplier";
    private bool m_animRunning = false;


    // --- Events ---
    public event Action OnActionStart;
    public event Action OnActionEnd;
    public event Action OnActionCooldownEnd;


    // --- Methods ---
    protected virtual void Awake()
    {
        m_actor = GetComponent<ActorController>();
        Debug.Assert(m_actor != null, $"ActorAction {name} requires an ActorController component.");

        m_animator = GetComponent<Animator>();
        Debug.Assert(m_animator != null, $"ActorAction {name} requires an Animator component.");
    }

    protected virtual void OnEnable()
    {
        // Subscribe to ActionEndSMB's event
        if (m_animator != null) {
            foreach (var behaviour in m_animator.GetBehaviours<ActionEndSMB>()) {
                m_actionEndSMB = behaviour;
                m_actionEndSMB.OnActionAnimEnd += ActionAnimEndHandler;
            }
        }

    }

    protected virtual void OnDisable()
    {
        // Unsubscribe from ActionEndSMB's event
        if (m_actionEndSMB != null) {
            m_actionEndSMB.OnActionAnimEnd -= ActionAnimEndHandler;
        }
    }

    protected virtual void Start()
    {
        // Set the animation clip for this action in the animator.
        if (m_animClip != null) {
            if (!m_actor.UpdateAnimClip(m_animClipName, m_animClip)) {
                Debug.LogError($"Failed to set animation clip for action {m_actionName}.");
            }
        }
    }

    public virtual void StartAction()
    {
        if (!m_actor.enableActions) {
            return;
        }

        if (!onCooldown && !m_animRunning) {
            onCooldown = true;
            OnActionStart?.Invoke();

            // Adjust animation play speed if the cooldown is faster than the animation
            float cooldown = 1f / actionsPerSecond;
            float speedMultiplier = m_attackAnimDuration > cooldown ? m_attackAnimDuration / cooldown : 1f;
            m_animator.SetFloat(m_speedMultiplierParamName, speedMultiplier);

            m_animRunning = true;
            m_animator.SetTrigger(m_animParamName);

            StartCoroutine(CooldownCR(cooldown));
        }
    }

    protected abstract void PerformAction();

    protected virtual IEnumerator CooldownCR(float cooldown)
    {
        if (cooldown > 0f) {
            yield return new WaitForSeconds(cooldown);
            OnActionCooldownEnd?.Invoke();
        }

        onCooldown = false;

        if (keepGoing) {
            StartAction();
        }
    }


    // --- Event Handlers ---
    protected virtual void ActionAnimEndHandler()
    {
        m_animRunning = false;

        OnActionEnd?.Invoke();

        if (keepGoing) {
            StartAction();
        }
    }

}
