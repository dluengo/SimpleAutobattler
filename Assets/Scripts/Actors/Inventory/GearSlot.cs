using System;
using UnityEngine;

[Serializable]
public class GearSlot
{
    // --- Members ---
    [SerializeField] protected BodyPart m_bodyPart;
    public BodyPart bodyPart => m_bodyPart;

    // NOTE: One could assign in inspector a Gear that doesn't match the body part.
    [SerializeField] protected Gear m_gear;
    public Gear gear
    {
        get => m_gear;
        set {
            if (value.bodyPart != m_bodyPart)
            {
                Debug.LogWarning($"Gear {value.itemName} cannot be equipped in slot for {m_bodyPart}.");
                return;
            }

            m_gear = value;
        }
    }


    // --- Methods ---
    public bool IsEmpty()
    {
        return m_gear.itemSO == null;
    }
}