using System;
using System.Collections.Specialized;
using UnityEngine;


[RequireComponent(typeof(ActorController))]
public abstract class Stat : MonoBehaviour
{
    // --- Members ---
    protected float m_value;
    public float value
    {
        get => m_value;
        set {
            float newValueClamped = clampAtMin ? Mathf.Max(value, minValue) : value;
            if (m_value != newValueClamped) {
                float oldValue = m_value;
                m_value = newValueClamped;

                // Trigger change event
                OnValueChanged?.Invoke();

                // Trigger zero event if applicable
                if (clampAtMin && m_value == minValue) {
                    OnValueMinimum?.Invoke();
                }
            }
        }
    }

    [Header("--- Stat Settings ---")]
    [SerializeField] protected StatSO m_statSO;
    public StatSO statSO
    {
        get => m_statSO;
        protected set => m_statSO = value;
    }

    protected bool clampAtMin => statSO != null && statSO.hasMinValue;
    protected float minValue => statSO != null ? statSO.minValue : 0f;

    protected ActorController m_actor;


    // --- Events ---
    public event Action OnValueChanged;
    public event Action OnValueMinimum;


    // --- Methods ---
    protected virtual void Awake()
    {
        m_actor = GetComponent<ActorController>();
        Debug.Assert(m_actor != null, "Stat requires an ActorController component on the same GameObject.");

        Debug.Assert(statSO != null, "Stat requires a reference to a StatSO ScriptableObject.");
    }

    protected virtual void OnEnable()
    {
        value = m_value;
        statSO = m_statSO;
    }

    protected abstract float CalculateStatValue();
}