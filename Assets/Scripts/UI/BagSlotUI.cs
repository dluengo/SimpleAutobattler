using UnityEngine;
using UnityEngine.UI;

public class BagSlotUI : UISlot<BagSlot, Item>
{
    // --- Members ---
    public BagSlot bagSlot
    {
        get => slot;
        set => slot = value;
    }

    protected BagSlot m_bagSlot => m_slot;
}