using System;
using System.ComponentModel;
using UnityEngine;

[Serializable]
public class GearSlot : SlotBase<Gear>
{
    // --- Members ---
    [SerializeField] protected BodyPart m_bodyPart;
    public BodyPart bodyPart => m_bodyPart;

    public Gear gear
    {
        get => element;
        set {
            if (value.bodyPart != m_bodyPart) {
                Debug.LogWarning($"Gear {value.itemName} cannot be equipped in slot for {m_bodyPart}.");
                return;
            }

            element = value;
        }
    }
}