using System;
using System.Collections.Specialized;
using UnityEngine;


[RequireComponent(typeof(ActorController))]
[Serializable]
public abstract class Stat : MonoBehaviour
{
    // --- Members ---
    [Header("--- Stat Settings ---")]
    [SerializeField] protected int m_currVal;
    public int currVal
    {
        get => m_currVal;
        protected set {
            int newValueClamped = clampAtMin ? Math.Max(minValue, value) : value;
            if (m_currVal != newValueClamped) {
                int oldValue = m_currVal;
                m_currVal = newValueClamped;

                // Trigger change event
                OnValueChanged?.Invoke();

                // Trigger minimum event if applicable
                if (clampAtMin && m_currVal == minValue) {
                    OnValueMinimum?.Invoke();
                }
            }
        }
    }

    [SerializeField] protected bool clampAtMin = true;
    [SerializeField] protected int minValue = 0;

    [SerializeField] protected StatSO m_statSO;
    //public StatSO statSO
    //{
    //    get => m_statSO;
    //    protected set => m_statSO = value;
    //}

    public Sprite icon => m_statSO.icon;

    protected ActorController m_actor;


    // --- Events ---
    public event Action OnValueChanged;
    public event Action OnValueMinimum;


    // --- Methods ---
    protected virtual void Awake()
    {
        m_actor = GetComponent<ActorController>();
        Debug.Assert(m_actor != null, $"{gameObject.name}:Awake(): Stat requires an ActorController component on the same GameObject.");

        Debug.Assert(m_statSO != null, $"{gameObject.name}:Awake(): Stat requires a reference to a StatSO ScriptableObject.");
    }

    protected virtual void OnEnable()
    {
        currVal = m_currVal;
        //statSO = m_statSO;
    }

    // Allows edit in inspector.
    //protected virtual void OnValidate()
    //{
    //    currVal = m_currVal;
    //    statSO = m_statSO;
    //}

    protected abstract int CalculateStatValue();
}