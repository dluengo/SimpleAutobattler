using UnityEngine;
using System;
using System.Collections;


[Serializable]
public class Equipment : ICollection
{
    // --- Members ---
    [Header("--- Equipment Settings ---")]
    [SerializeField] protected GearSlot[] m_gearSlots;

    public int Count => m_gearSlots.Length;

    public bool IsSynchronized => throw new NotImplementedException();

    public object SyncRoot => throw new NotImplementedException();


    // --- Methods ---
    public IEnumerator GetEnumerator()
    {
        return m_gearSlots.GetEnumerator();
    }

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

    public void CopyTo(Array array, int index)
    {
        for (int i = index; i < array.Length; i++) {
            array.SetValue(m_gearSlots[i], i);
        }
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
