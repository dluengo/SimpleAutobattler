using System.Collections;
using UnityEngine;

// NOTE: Inherit from UIBase not because this class displays anything on screen,
// but because we need it to run Awake() after the Equipment in the player has
// been initialized. UIBase has a lower priority than Default in Script Execution Order
public class EquipmentUI : UIBase
{
    // --- Members ---
    [SerializeField] protected InventoryController m_inventory;

    public Equipment equipment => m_inventory.equipment;

    protected GearSlotUI[] m_gearSlotUIList;


    // --- Methods ---
    private void Awake()
    {
        Debug.Assert(m_inventory != null, "EquipmentUI: Inventory is not assigned.");

        // NOTE: Children of this gameobject *SHOULD* be GearSlotUI objects.
        // Also number of GearSlotUI and GearSlot *SHOULD* match.
        if (transform.childCount != equipment.Count) {
            Debug.LogWarning($"EquipmentUI: Number of GearSlotUI children ({transform.childCount}) does not match number of GearSlots in Equipment ({equipment.Count}).");
            return;
        }

        SetupSlots();
    }

    public override void UpdateUI() {
        foreach (GearSlotUI slotUI in m_gearSlotUIList) {
            if (slotUI != null) {
                slotUI.UpdateUI();
            }
        }
    }


    // --- Helper Methods ---

    // NOTE: number of GearSlotUI and GearSlot *SHOULD* match.
    protected void SetupSlots()
    {
        m_gearSlotUIList = new GearSlotUI[transform.childCount];

        int index = 0;
        foreach (GearSlot slot in equipment) {
            m_gearSlotUIList[index] = transform.GetChild(index).GetComponent<GearSlotUI>();
            if (m_gearSlotUIList[index] == null) {
                Debug.LogWarning($"EquipmentUI: Child at index {index} does not have a GearSlotUI component.");
                index++;
                continue;
            }

            m_gearSlotUIList[index].slot = slot;

            index++;
        }
    }

    protected void SetupEquipmentUI()
    {

        // Initialize each GearSlotUI with a GearSlot from the Equiment.
        // NOTE: The number of GearSlotUIs and GearSlots *SHOULD* match.
        BitArray assigned = new BitArray(equipment.Count, false);
        foreach (GearSlotUI slotUI in m_gearSlotUIList) {
            if (slotUI != null) {
                // NOTE: Equipment may have multiple GearSlots for the same BodyPart,
                // imagine a character with 2 hands, 2 rings, etc... You can go even
                // further adding more heads or whatever.
                //
                // We keep track of which GearSlots have been already assigned to a GearSlotUI
                // by using a BitArray to mark when a GearSlot has been assigned.
                int slotIndex = 0;
                foreach (GearSlot gearSlot in equipment) {
                    if (slotUI.bodyPart == gearSlot.bodyPart && !assigned[slotIndex]) {
                        slotUI.slot = gearSlot;
                        assigned.Set(slotIndex, true);
                        break;
                    }
                    slotIndex++;
                }
            }
            else {
                Debug.LogWarning("EquipmentUI: Child of m_gearSlotUIList does not have a GearSlotUI component.");
            }
        }
    }
}
