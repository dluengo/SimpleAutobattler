using UnityEngine;

public abstract class StatUI<T> : UIBase where T : Stat
{
    // --- Members ---
    [Header("--- StatUI Settings ---")]
    //[SerializeField] StatsController stats;

    [SerializeField] protected T stat;


    // --- Methods ---
    protected abstract void Awake();

    protected virtual void OnEnable()
    {
        if (stat != null) {
            stat.OnValueChanged += UpdateUI;
            //stat.OnMaxValueChanged += UpdateUI;
        }
    }

    protected virtual void OnDisable()
    {
        if (stat != null) {
            stat.OnValueChanged -= UpdateUI;
            //stat.OnMaxValueChanged -= UpdateUI;
        }
    }

    // NOTE: OnValueMaxChanged needs an Action<int, int>. We implement our own
    // UpdateUI(int, int) and wrap it around the parameterless UpdateUI().
    protected virtual void UpdateUI(int oldValue, int newValue)
    {
        UpdateUI();
    }
}
