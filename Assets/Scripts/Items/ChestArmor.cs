using UnityEngine;

public class ChestArmor : Gear
{
    // --- Members ---
    public int defense => m_chestArmorSO.defense;

    protected ChestArmorSO m_chestArmorSO => m_gearSO as ChestArmorSO;


    // --- Methods ---
    public ChestArmor(ChestArmorSO chestArmorSO) : base(chestArmorSO) {}
}
