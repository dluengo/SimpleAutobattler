using UnityEngine;
using System;

public class Damage : Stat
{
    // --- Members ---
    [Header("--- Damage Settings ---")]
    [SerializeField] float totalDamage;

    [SerializeField] protected int m_baseDamage;
    public int baseDamage
    {
        get => m_baseDamage;
        protected set {
            if (m_baseDamage != value) {
                m_baseDamage = value;
                this.value = CalculateDamage();
            }
        }
    }

    [SerializeField] protected int m_damagePerStr;
    public int damagePerStr
    {
        get => m_damagePerStr;
        protected set {
            if (m_damagePerStr != value) {
                m_damagePerStr = value;
                this.value = CalculateDamage();
            }
        }
    }

    // It is ok if the Stat doesn't have a reference to the strength attribute
    // of an Actor, but if it does, it will use it to calculate the total damage.
    protected Strength m_strength;


    // --- Events ---


    // --- Methods ---
    protected override void OnEnable()
    {
        base.OnEnable();

        baseDamage = m_baseDamage;

        if (m_strength != null) {
            m_strength.OnValueChanged -= OnStrengthChangedHandler;
        }

        this.OnValueChanged += UpdateTotalDamage;
    }

    protected virtual void Start()
    {
        m_strength = m_actor.GetAttribute<Strength>();

        // Subscribe to strength changes to update damage accordingly.
        if (m_strength != null) {
            m_strength.OnValueChanged += OnStrengthChangedHandler;
        }

        // Don't trigger events.
        m_value = CalculateDamage();

        totalDamage = m_value;
    }

    protected override float CalculateStatValue()
    {
        return CalculateDamage();
    }


    // --- Event Handlers ---
    private void OnStrengthChangedHandler()
    {
        value = CalculateDamage();
    }

    public void UpdateTotalDamage()
    {
        totalDamage = CalculateDamage();
    }


    // --- Helpers ---
    private float CalculateDamage()
    {
        // Damage is baseDamage + strength * m_damagePerStr
        return baseDamage + (m_strength != null ? m_strength.value * m_damagePerStr : 0);
    }
}
