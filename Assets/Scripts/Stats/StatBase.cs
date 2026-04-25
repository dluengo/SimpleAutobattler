using UnityEngine;
using System;

public abstract class StatBase : MonoBehaviour
{
    // --- Members ---
    [SerializeField] string m_statName;
    public string statName {
        get => m_statName;
        protected set => m_statName = value;
    }
    [SerializeField] float m_maxValue = 100f;
    public float maxValue {
        get => m_maxValue;
        protected set => m_maxValue = value;
    }
    [SerializeField] float m_minValue = 0f;
    public float minValue {
        get => m_minValue;
        protected set => m_minValue = value;
    }
    //[SerializeField] float m_initialValue;
    //public float initialValue {
    //    get => m_initialValue;
    //    protected set => m_initialValue = value;
    //}

    private float m_currentValue;
    public float currentValue
    {
        get => m_currentValue;
        protected set {
            if (value != m_currentValue) {

                if (value < minValue || (value > maxValue && maxValue >= 0)) {
                    Debug.LogWarning($"Attempted to set currentValue to {value}, which is outside the range [{minValue}, {maxValue}]. Clamping to valid range.");
                }

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


    // --- Events ---
    public event Action OnValueChanged;
    public event Action OnValueMin;
    public event Action OnValueMax;


    // --- Methods ---
    protected virtual void Awake()
    {
        currentValue = maxValue;
    }
}
