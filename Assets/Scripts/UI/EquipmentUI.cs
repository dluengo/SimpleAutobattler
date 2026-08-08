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

        SetupSlotUIList();

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
    protected void SetupSlotUIList()
    {
        // Initialize the list of GearSlotUI components based on the number of children.
        // The children *SHOULD* have GearSlotUI components attached to them and the
        // bodyParts of these UIs *SHOULD* match the bodyParts of the GearSlots in the Equipment.
        m_gearSlotUIList = new GearSlotUI[transform.childCount];

        for (int i = 0; i < transform.childCount; i++) {

            m_gearSlotUIList[i] = transform.GetChild(i).GetComponent<GearSlotUI>();
            if (m_gearSlotUIList[i] == null) {
                Debug.LogWarning($"EquipmentUI: Child at index {i} does not have a GearSlotUI component.");
            }
        }
    }

    protected void SetupSlots()
    {
        // We need to assign a GearSlotUI to each GearSlot in the Equipment.
        // For this, the number of GearSlotUI set in the Editor and the bodyParts
        // of each them *SHOULD* match the number of GearSlots in the Equipment
        // and their bodyParts.
        foreach (GearSlot slot in equipment) {
            bool slotUIFound = false;

            // Iterate through the GearSlotUIs looking for a proper candidate
            // to match this GearSlot.
            for (int index = 0; index < m_gearSlotUIList.Length; index++) {

                GearSlotUI candidateSlotUI = m_gearSlotUIList[index];

                // Candidate must exist.
                if (candidateSlotUI != null) {

                    // Candidate must not have been assigned to a GearSlot yet.
                    if (candidateSlotUI.slot == null) {

                        // Candidate must have the same bodyPart as the GearSlot.
                        if (candidateSlotUI.bodyPart == slot.bodyPart) {                            
                            candidateSlotUI.slot = slot;
                            slotUIFound = true;
                            break;
                        }
                    }
                }
                else {
                    Debug.LogWarning($"EquipmentUI: Child at index {index} does not have a GearSlotUI component.");
                }
            }


            if (!slotUIFound) {
                Debug.LogWarning($"EquipmentUI: No GearSlotUI found for GearSlot with bodyPart {slot.bodyPart}.");
            }
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
