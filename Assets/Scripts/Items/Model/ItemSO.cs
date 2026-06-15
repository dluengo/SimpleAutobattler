using UnityEngine;

public abstract class ItemSO : ScriptableObject
{
    // --- Members ---
    [Header("--- itemSO Settings ---")]
    public string itemName;
    [TextArea]
    public string description;
    public Sprite icon;
    public AnimationClip idleClip;
    public AnimationClip pickedUpClip;


    // --- Methods ---
    public abstract Item CreateNewItem();
}
