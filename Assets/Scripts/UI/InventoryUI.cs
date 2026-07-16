using UnityEngine;


// NOTE: This class doesn't display anything on screen, but we want it to be UIBase
// so it Script Execution Order is after the InventoryController is initialized.
public class InventoryUI : UIBase
{
    // --- Members ---
    [Header("--- Inventory UI Settings ---")]
    [SerializeField] protected InventoryController m_inventory;
    [SerializeField] protected EquipmentUI m_equipmentUI;
    [SerializeField] protected BagUI m_bagUI;


    // --- Methods ---
    private void Awake()
    {
        Debug.Assert(m_inventory != null, "InventoryUI: InventoryController is not assigned.");
        Debug.Assert(m_equipmentUI != null, "InventoryUI: EquipmentUI is not assigned.");
        Debug.Assert(m_bagUI != null, "InventoryUI: BagUI is not assigned.");

        //m_equipmentUI.equipment = m_inventory.equipment;
        //m_bagUI.bag = m_inventory.bag;
    }

    //protected override void Start()
    //{
    //    base.Start();

    //    //Debug.Assert(m_equipmentUI != null, "InventoryUI: EquipmentUI is not assigned.");
    //    //Debug.Assert(m_bagUI != null, "InventoryUI: BagUI is not assigned.");

    //    //m_equipmentUI.equipment = m_inventory.equipment;
    //    //m_bagUI.bag = m_inventory.bag;
    //}

    public override void UpdateUI() {
        m_equipmentUI.UpdateUI();
        m_bagUI.UpdateUI();
    }
}
