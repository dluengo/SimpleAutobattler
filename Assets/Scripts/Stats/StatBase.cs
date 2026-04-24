using UnityEngine;
using System;

public abstract class StatBase : MonoBehaviour
{
    // --- Members ---
    public string statName { get; protected set; }

    private float m_currentValue;
    public float currentValue {
        get => m_currentValue;
        protected set { 
            if (value != m_currentValue) {

                if (value < minValue || (value > maxValue && maxValue >= 0)) {
                    Debug.LogWarning($"Attempted to set currentValue to {value}, which is outside the range [{minValue}, {maxValue}]. Clamping to valid range.");
                }

                m_currentValue = Mathf.Clamp(value, minValue, maxValue);
                OnValueChanged?.Invoke();
            }
        }
    }
    public float maxValue { get; protected set; }
    public float minValue { get; protected set; }

    [SerializeField] protected float initialValue;

    // --- Events ---
    public event Action OnValueChanged;
}
