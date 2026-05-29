using UnityEngine;

public abstract class StatUIBase : UIBase
{
    // --- Members ---
    // NOTE: This member must be assigned in the derived class's Awake() method,
    // after the specific StatBase component is validated.
    [Header("--- Stat UI Settings ---")]
    [SerializeField] protected StatBase stat;


    // --- Methods ---
    protected abstract void Awake();

    protected virtual void OnEnable()
    {
        if (stat != null) {
            stat.OnValueChanged += UpdateUI;
            stat.OnMaxValueChanged += UpdateUI;
        }
    }

    protected virtual void OnDisable()
    {
        if (stat != null) {
            stat.OnValueChanged -= UpdateUI;
            stat.OnMaxValueChanged -= UpdateUI;
        }
    }
}
