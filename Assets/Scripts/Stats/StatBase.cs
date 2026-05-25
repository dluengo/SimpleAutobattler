using UnityEngine;
using System;

[RequireComponent(typeof(ActorController))]
public abstract class StatBase : MonoBehaviour
{
    // --- Members ---
    [Header("--- StatBase Settings ---")]
    protected string m_statName;
    [SerializeField] int m_maxValue = 100;
    public int maxValue {
        get => m_maxValue;
        protected set => m_maxValue = value;
    }
    [SerializeField] int m_minValue = 0;
    public int minValue {
        get => m_minValue;
        protected set => m_minValue = value;
    }

    private int m_currentValue;
    public int currentValue
    {
        get => m_currentValue;
        protected set {
            if (value != m_currentValue) {

                if (value < minValue || (value > maxValue && maxValue >= 0)) {
                    Debug.LogWarning($"Attempted to set currentValue to {value}, which is outside the range [{minValue}, {maxValue}]. Clamping to valid range.");
                }

                float oldValue = m_currentValue;
                m_currentValue = Mathf.Clamp(value, minValue, maxValue);
                OnValueChanged?.Invoke();

                if (m_currentValue <= minValue) {
                    OnValueMin?.Invoke();
                }
                else if (maxValue >= 0 && m_currentValue >= maxValue) {
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

    public event Action OnMaxValueChanged;


    // --- Methods ---
    protected virtual void Awake()
    {
        m_actor = GetComponent<ActorController>();
        Debug.Assert(m_actor != null, "StatBase: ActorController component is missing.");
    }

    protected virtual void Start()
    {
        currentValue = maxValue;
    }
}
