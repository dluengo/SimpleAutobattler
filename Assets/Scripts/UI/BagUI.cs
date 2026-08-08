using UnityEngine;

public class BagUI : UIBase
{
    // --- Members ---
    // NOTE: To be set from outside.
    [SerializeField] protected InventoryController m_inventory;

    public Bag bag => m_inventory.bag;

    protected BagSlotUI[] m_bagSlotUIList;


    // --- Methods ---
    private void Awake()
    {
        Debug.Assert(m_inventory != null, "BagUI: InventoryController is not assigned.");

        // NOTE: Children of this gameobject *SHOULD* be BagSlotUI objects.
        // Also number of BagSlotUI and bag.Count *SHOULD* match.
        if (transform.childCount != bag.Count) {
            Debug.LogWarning($"BagUI: Number of BagSlotUI children ({transform.childCount}) does not match number of BagSlots in Bag ({bag.Count}).");
            return;
        }

        SetupSlots();
    }

    public override void UpdateUI() {
        for (int i = 0; i < m_bagSlotUIList.Length; i++) {
            BagSlotUI bagSlotUI = m_bagSlotUIList[i];
            if (bagSlotUI != null) {
                bagSlotUI.UpdateUI();
            }
        }
    }


    // --- Helper Methods ---

    // NOTE: The number of slots in the bag and the number of BagSlotUI in the
    // Canvas *SHOULD* match.
    protected void SetupSlots()
    {
        m_bagSlotUIList = new BagSlotUI[transform.childCount];

        int index = 0;
        foreach (BagSlot bagslot in bag) {
            m_bagSlotUIList[index] = transform.GetChild(index).GetComponent<BagSlotUI>();
            if (m_bagSlotUIList[index] == null) {
                Debug.LogWarning($"BagUI: Child at index {index} does not have a BagSlotUI component.");
                index++;
                continue;
            }

            m_bagSlotUIList[index].slot = bagslot;

            index++;
        }
    }
}