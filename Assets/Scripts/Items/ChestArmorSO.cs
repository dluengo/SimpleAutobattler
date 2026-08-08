using UnityEngine;

[CreateAssetMenu(fileName = "ChestArmorSO", menuName = "Items/Gear/ChestArmorSO")]
public class ChestArmorSO : GearSO
{
    // --- Members ---
    [Header("--- ChestArmorSO Settings ---")]
    public int defense = 1;


    // --- Methods ---
    public override Item CreateNewItem()
    {
        return new ChestArmor(this);
    }
}
