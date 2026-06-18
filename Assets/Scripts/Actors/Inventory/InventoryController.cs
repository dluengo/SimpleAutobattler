using System;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    // --- Members ---
    [SerializeField] protected Bag m_bag;
    [SerializeField] protected Equipment m_equipment;

    //public Bag Bag => m_bag;
    //public Equipment Equipment => m_equipment;


    // --- Methods ---
    // When adding an item to the inventory, if it is a gear we try to equip it,
    // otherwise we try to add it to the bag.
    public bool AddItem(Item item)
    {
        // If the item is a gear, try to equip it first
        if (item is Gear gear) {
            Debug.Log($"Trying to equip gear: {gear.itemName}");
            if (!m_equipment.EquipGear(gear)) {
                Debug.Log($"Failed to equip gear: {gear.itemName}, trying to add it to bag instead.");
                return m_bag.AddItem(item);
            }

            // Gear was successfully equipped
            return true;
        }
        // Item is not a gear, try to add it to the bag
        else {
            return m_bag.AddItem(item);
        }
    }
}