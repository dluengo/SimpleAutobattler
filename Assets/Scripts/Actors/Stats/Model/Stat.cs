using System;
using System.Collections.Specialized;
using UnityEngine;


[RequireComponent(typeof(ActorController))]
public abstract class Stat : MonoBehaviour
{
    // --- Members ---
    [SerializeField]protected int m_value;
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

    //protected StatSO m_statSO;
    [SerializeField]protected string m_statName;
    public string statName => m_statName;
    [SerializeField, TextArea] protected string m_statDescription;
    public string statDescription => m_statDescription;
    [SerializeField] protected Sprite m_icon;
    public Sprite icon => m_icon;

    protected ActorController m_actor;


    // --- Events ---
    public event Action<int, int> OnValueChanged;
    public event Action OnValueZero;


    // --- Methods ---
    protected virtual void Awake()
    {
        m_actor = GetComponent<ActorController>();
        Debug.Assert(m_actor != null, "Stat requires an ActorController component on the same GameObject.");
    }

    protected virtual void OnEnable()
    {
        value = m_value;
    }
}