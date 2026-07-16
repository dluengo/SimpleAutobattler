using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GearSlotUI : UISlot<GearSlot, Gear>
{
    // --- Members ---
    [Header("--- GearSlotUI Settings ---")]
    [SerializeField] protected BodyPart m_bodyPart;
    public BodyPart bodyPart => m_bodyPart;

    protected GearSlot m_gearSlot => m_slot;
}
