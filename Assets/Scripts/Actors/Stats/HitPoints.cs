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
    [SerializeField] protected int m_hpPerVit;
    public int HPPerVit
    {
        get => m_hpPerVit;
        protected set {
            if (m_hpPerVit != value) {
                m_hpPerVit = value;
                this.value = CalculateMaxHP();
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
    private bool m_alreadySubscribed = false;

    // NOTE: This member is just to be able to watch in the inspector how
    // the hp actually changes.
    [SerializeField] private int m_currentHP;
    public int currentHPReadOnly
    {
        get => (int)value;
    }


    // --- Events ---
    public event Action<float> OnDamageTaken;
    public event Action OnMaxHPChanged;
    public event Action OnBaseHPChanged;
    public event Action OnDeath;


    // --- Methods ---
    //protected override void OnEnable()
    //{
    //    base.OnEnable();

    //    SubscribeEvents();
    //}

    protected virtual void OnDisable()
    {
        UnsubscribeEvents();
    }

    protected virtual void Start()
    {
        m_vitality = m_actor.GetAttribute<Vitality>();

        maxHP = CalculateMaxHP();
        value = maxHP;

        // Subscribe to vitality changes to update HP accordingly.
        SubscribeEvents();
    }

    public void TakeDamage(float damage)
    {
        if (damage == 0) {
            return;
        }

        // Could this cast and next comparison cause issues?
        int newHP = Mathf.FloorToInt(value - damage);
        if (newHP != value) {
            OnDamageTaken?.Invoke(damage);
        }

        // Update the current value.
        //
        // NOTE: If the min values are set, the OnValueMinimum event will trigger,
        // which will trigger our OnDeath event.
        value = newHP;
    }

    protected override float CalculateStatValue()
    {
        return CalculateMaxHP();
    }


    // --- Event Handlers ---
    private void OnVitalityChangedHandler()
    {
        maxHP = CalculateMaxHP();
        value = Mathf.Min(value, maxHP);
    }

    private void UpdateCurrentHP()
    {
        m_currentHP = (int)value;
    }


    // --- Helpers ---
    private int CalculateMaxHP()
    {
        // NOTE: Could there be issues with the casting?
        // MaxHP is baseDamage + vitality.
        return baseHP + (m_vitality != null ? (int)(m_vitality.value * m_hpPerVit) : 0);
    }

    private void SubscribeEvents()
    {
        this.OnValueChanged += UpdateCurrentHP;

        if (!m_alreadySubscribed) {
            if (m_vitality != null) {
                m_vitality.OnValueChanged += OnVitalityChangedHandler;
            }

            // Invoke the OnDeath event (event of HitPoints) when the Stat reaches its
            // minimum. Note OnValueMinimum is invoked by Stat.
            //
            // This could be considered a renaming of the OnValueMinimum event to give
            // some meaningful name to this layer of abstraction.
            OnValueMinimum += () => { OnDeath?.Invoke(); };
            m_alreadySubscribed = true;
        }
    }

    private void UnsubscribeEvents()
    {
        this.OnValueChanged -= UpdateCurrentHP;

        if (m_vitality != null) {
            m_vitality.OnValueChanged -= OnVitalityChangedHandler;
        }
    }
}
