using UnityEngine;
using System;


[Serializable]
public class Equipment
{
    // --- Members ---
    [SerializeField] protected GearSlot[] m_gearSlots;


    // --- Methods ---
    public bool EquipGear(Gear gear)
    {
        // Find the gear slot for the body part of the gear
        GearSlot gearSlot = GetEmptyGearSlot(gear.bodyPart);

        // Equip the gear if we have an empty slot for it.
        if (gearSlot != null) {
            gearSlot.gear = gear;
            return true;
        }

        return false;
    }


    // -- Helper Methods ---
    protected GearSlot GetEmptyGearSlot(BodyPart bodyPart)
    {
        foreach (GearSlot gearSlot in m_gearSlots) {
            if (gearSlot.bodyPart == bodyPart && gearSlot.IsEmpty()) {
                return gearSlot;
            }
        }

        return null;
    }
}
