using UnityEngine;

public abstract class StatUIBase : MonoBehaviour
{
    // --- Members ---
    // NOTE: This member must be assigned in the derived class's Awake() method,
    // after the specific StatBase component is validated.
    protected StatBase stat;


    // --- Methods ---
    protected virtual void Awake()
    {
        Debug.Assert(stat != null, "Stat is not assigned.");
    }

    protected virtual void OnEnable()
    {
        stat.OnValueChanged += UpdateUI;
    }

    protected virtual void OnDisable()
    {
        stat.OnValueChanged -= UpdateUI;
    }

    protected virtual void Start()
    {
        UpdateUI();
    }

    protected abstract void UpdateUI();
}
