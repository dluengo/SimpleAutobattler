using System;
using System.Collections;
using UnityEngine;


[Serializable]
public class Bag : IEnumerable
{
    // --- Members ---
    //[SerializeField] protected int m_nSlots;
    ////public int nSlots => m_nSlots;

    [SerializeField] protected Item[] m_itemSlots;
    public Item[] items => m_itemSlots;


    // --- Methods ---
    public IEnumerator GetEnumerator()
    {
        return m_itemSlots.GetEnumerator();
    }

    public bool AddItem(Item item)
    {
        // Find the first empty slot in the bag and add the item to that slot.
        for (int i = 0; i < m_itemSlots.Length; i++) {
            if (m_itemSlots[i] == null) {
                m_itemSlots[i] = item;
                return true;
            }
        }

        // No empty slots available
        return false;
    }
}
