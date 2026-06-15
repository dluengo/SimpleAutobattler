using UnityEngine;

[CreateAssetMenu(fileName = "BowSO", menuName = "Items/Gear/BowSO")]
public class BowSO : GearSO
{
    // --- Members ---
    [Header("--- BowSO Settings ---")]
    public int damage = 10;
    public int range = 5;


    // --- Methods ---
    public override Item CreateNewItem()
    {
        return new Bow(this);
    }
}
