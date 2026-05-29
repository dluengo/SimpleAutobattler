using UnityEngine;
using System;


[RequireComponent(typeof(ActorController))]
public abstract class StatBase : MonoBehaviour
{
    // --- Members ---
    [Header("--- StatBase Settings ---")]
    [SerializeField] bool m_hasMin;
    public bool hasMin
    {
        get => m_hasMin;
        protected set => m_hasMin = value;
    }
    [SerializeField] int m_minValue;
    public int minValue
    {
        get => m_minValue;
        protected set {
            if (m_minValue != value) {
                m_minValue = value;

                if (hasMin) {
                    OnMinValueChanged?.Invoke();
                }
            }
        }
    }

    [SerializeField] bool m_hasMax;
    public bool hasMax
    {
        get => m_hasMax;
        protected set => m_hasMax = value;
    }
    [SerializeField] int m_maxValue;
    public int maxValue
    {
        get => m_maxValue;
        protected set {
            if (m_maxValue != value) {
                m_maxValue = value;

                if (hasMax) {
                    OnMaxValueChanged?.Invoke();
                }
            }
        }
    }

    [SerializeField] int m_currentValue;
    public int currentValue
    {
        get => m_currentValue;
        protected set {
            if (m_currentValue != value) {
                if (hasMin && hasMax) {
                    m_currentValue = Math.Clamp(value, minValue, maxValue);
                }
                else if (hasMin) {
                    m_currentValue = Math.Max(minValue, value);
                }
                else if (hasMax) {
                    m_currentValue = Math.Min(value, maxValue);
                }
                // Both min and max are disabled
                else {
                    m_currentValue = value;
                }

                // Trigger event.
                OnValueChanged?.Invoke();

                // Check if extra events need to be triggered.
                if (hasMin && m_currentValue <= minValue) {
                    OnValueMin?.Invoke();
                }
                else if (hasMax && m_currentValue >= maxValue) {
                    OnValueMax?.Invoke();
                }
            }
        }
    }

    protected ActorController m_actor;


    // --- Events ---
    public event Action OnValueChanged;
    public event Action OnValueMin;
    public event Action OnValueMax;

    public event Action OnMinValueChanged;
    public event Action OnMaxValueChanged;


    // --- Methods ---
    protected virtual void Awake()
    {
        m_actor = GetComponent<ActorController>();

        Debug.Assert(m_actor != null, "StatBase: Actor reference is not assigned.");
    }
}
