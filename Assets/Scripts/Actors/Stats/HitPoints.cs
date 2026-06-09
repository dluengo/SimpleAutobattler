using UnityEngine;
using System;
using Unity.VisualScripting;


public class HitPoints : Stat
{
    // --- Members ---
    [Header("--- Hit Points Settings ---")]
    [SerializeField] protected int m_baseHP;
    public int baseHP
    {
        get => m_baseHP;
        protected set
        {
            if (m_baseHP != value) {
                m_baseHP = value;
                OnBaseHPChanged?.Invoke();
            }
        }
    }

    protected int m_maxHP;
    public int maxHP
    {
        get => m_maxHP;
        protected set {
            if (m_maxHP != value) {
                m_maxHP = value;
                OnMaxHPChanged?.Invoke();
            }
        }
    }

    // It is ok if the Stat doesn't have a reference to the vitality attribute
    // of an Actor, but if it does, it will use it to calculate max HP.
    protected Vitality m_vitality;


    // --- Events ---
    public event Action OnMaxHPChanged;
    public event Action OnBaseHPChanged;


    // --- Methods ---
    protected virtual void Start()
    {
        m_vitality = m_actor.GetAttribute<Vitality>();

        maxHP = value = CalculateMaxHP();

        // Subscribe to vitality changes to update HP accordingly.
        if (m_vitality != null) {
            m_vitality.OnValueChanged += OnVitalityChangedHandler;
        }
    }

    public void ChangeHP(float amount)
    {
        if (amount == 0) {
            return;
        }

        // Amount is float but HP is int, we need to handle this.
        int newHP = Mathf.RoundToInt(value - amount);
        if (newHP != value) {
            
        }

        // Update the current value.
        value = newHP;
    }


    // --- Event Handlers ---
    private void OnVitalityChangedHandler(int oldValue, int newValue)
    {
        maxHP = CalculateMaxHP();
        value = Mathf.Min(value, maxHP);
    }


    // --- Helpers ---
    private int CalculateMaxHP()
    {
        // MaxHP is baseHP + vitality.
        return baseHP + (m_vitality != null ? m_vitality.value : 0);
    }
}
