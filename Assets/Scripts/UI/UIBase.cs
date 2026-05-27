using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    // --- Members ---
    //[Header("--- UI Settings ---")]


    // --- Methods ---
    // NOTE: When overriding, base.Start() should be called after any
    // initialization to ensure the UI updates properly.
    protected virtual void Start()
    {
        UpdateUI();
    }

    public abstract void UpdateUI();
}
