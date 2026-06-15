using UnityEngine;

public abstract class Gear : Item
{
    // --- Members ---
    public BodyPart bodyPart => m_gearSO.bodyPart;

    protected GearSO m_gearSO => m_itemSO as GearSO;


    // --- Methods ---
    protected Gear(GearSO gearSO) : base(gearSO) {}
}
