using UnityEngine;
using UnityEngine.UI;

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


    // --- Helper Methods ---
    protected static void SetTransparent(Image image, bool transparent)
    {
        Color color = image.color;

        // Set transparent or opaque.
        color.a = transparent ? 0f : 1f;
        image.color = color;
    }
}
