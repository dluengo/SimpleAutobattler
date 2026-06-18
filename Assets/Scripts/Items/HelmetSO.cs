using UnityEngine;

[CreateAssetMenu(fileName = "HelmetSO", menuName = "Items/Gear/HelmetSO")]
public class HelmetSO : GearSO
{
    // --- Members ---
    [Header("--- HelmetSO Settings ---")]
    public int baseDefense;


    // --- Methods ---
    public override Item CreateNewItem()
    {
        return new Helmet(this);
    }
}