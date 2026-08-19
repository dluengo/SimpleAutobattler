using UnityEngine;
using System;
using Unity.VisualScripting;


public class HitPoints : Stat
{
    // --- Members ---
    [Header("--- Hit Points Settings ---")]
    [SerializeField] protected int m_baseHP = 1;
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

    [SerializeField] protected int m_hpPerVit = 1;
    public int HPPerVit
    {
        get => m_hpPerVit;
        protected set {
            if (m_hpPerVit != value) {
                m_hpPerVit = value;
                this.currVal = CalculateMaxHP();
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
    protected Vitality m_vitStat;
    private bool m_alreadySubscribed = false;


    // --- Events ---
    public event Action<float> OnDamageTaken;
    public event Action OnMaxHPChanged; 
    public event Action OnBaseHPChanged;
    public event Action OnDeath;


    // --- Methods ---
    protected override void Awake()
    {
        base.Awake();

        m_vitStat = gameObject.GetComponent<Vitality>();
    }

    protected virtual void OnDisable()
    {
        UnsubscribeEvents();
    }

    protected virtual void Start()
    {
        maxHP = CalculateMaxHP();
        currVal = maxHP;

        // Subscribe to vitality changes to update HP accordingly.
        SubscribeEvents();
    }

    public void TakeDamage(int damage)
    {
        if (damage == 0) {
            return;
        }

        // Could this cast and next comparison cause issues?
        int newHP = currVal - damage;
        if (newHP != currVal) {
            OnDamageTaken?.Invoke(damage);
        }

        // Update the current value.
        //
        // NOTE: If the min values are set, the OnValueMinimum event will trigger,
        // which will trigger our OnDeath event.
        currVal = newHP;
    }

    protected override int CalculateStatValue()
    {
        return CalculateMaxHP();
    }


    // --- Event Handlers ---
    private void OnVitalityChangedHandler()
    {
        maxHP = CalculateMaxHP();
        currVal = Mathf.Min(currVal, maxHP);
    }


    // --- Helpers ---
    private int CalculateMaxHP()
    {
        // MaxHP is baseDamage + vitality.
        return baseHP + (m_vitStat != null ? (m_vitStat.currVal * m_hpPerVit) : 0);
    }

    private void SubscribeEvents()
    {
        if (!m_alreadySubscribed) {
            if (m_vitStat != null) {
                m_vitStat.OnValueChanged += OnVitalityChangedHandler;
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
        if (m_vitStat != null) {
            m_vitStat.OnValueChanged -= OnVitalityChangedHandler;
        }
    }
}
