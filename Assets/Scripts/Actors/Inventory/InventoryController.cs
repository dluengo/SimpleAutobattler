using System;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    // --- Members ---
    [Header("--- Inventory Settings ---")]
    [SerializeField] protected Bag m_bag;
    public Bag bag => m_bag;
    [SerializeField] protected Equipment m_equipment;
    public Equipment equipment => m_equipment;

    //public bag bag => m_bagUI;
    //public equipment equipment => equipment;


    // --- Methods ---
    //private void Awake()
    //{
    //    Debug.Assert(m_bag != null, "InventoryController: Bag is not assigned.");
    //    Debug.Assert(m_equipment != null, "InventoryController: Equipment is not assigned.");
    //}


    // When adding an item to the inventory, if it is a gear we try to equip it,
    // otherwise we try to add it to the bag.
    public bool AddItem(Item item)
    {
        // If the item is a gear, try to equip it first
        if (item is Gear gear) {
            Debug.Log($"Trying to equip gear: {gear.itemName}");
            if (!m_equipment.EquipGear(gear)) {
                Debug.Log($"Failed to equip gear: {gear.itemName}, trying to add it to bag instead.");
                return bag.AddItem(item);
            }

            // Gear was successfully equipped
            return true;
        }
        // m_item is not a gear, try to add it to the bag
        else {
            return bag.AddItem(item);
        }
    }
}