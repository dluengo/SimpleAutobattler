using System;
using System.Collections.Specialized;
using UnityEngine;


[RequireComponent(typeof(ActorController))]
public abstract class Stat : MonoBehaviour
{
    // --- Members ---
    protected int m_value;
    public int value
    {
        get => m_value;
        set {
            if (m_value != value) {
                int oldValue = m_value;
                m_value = value;

                // Trigger change event
                OnValueChanged?.Invoke(oldValue, m_value);

                // Trigger zero event if applicable
                if (m_value == 0 && oldValue != 0) {
                    OnValueZero?.Invoke();
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

    protected ActorController m_actor;


    // --- Events ---
    public event Action<int, int> OnValueChanged;
    public event Action OnValueZero;


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

    protected abstract int CalculateStatValue();
}