using System;
using System.Collections;
using UnityEngine;


[Serializable]
public class Bag : ICollection
{
    // --- Members ---
    [Header("--- Bag Settings ---")]
    // NOTE: To be set from Inspector.
    [SerializeField] protected BagSlot[] m_bagSlots;
    public BagSlot[] items => m_bagSlots;

    public int Count => items.Length;

    public bool IsSynchronized => throw new NotImplementedException();

    public object SyncRoot => throw new NotImplementedException();


    // Define the indexer to allow client code to use [] notation.
    public BagSlot this[int i]
    {
        get => m_bagSlots[i];
        set => m_bagSlots[i] = value;
    }


    // --- Methods ---
    public IEnumerator GetEnumerator()
    {
        return m_bagSlots.GetEnumerator();
    }

    public bool AddItem(Item item)
    {
        BagSlot bagSlot = GetEmptyBagSlot();

        if (bagSlot != null) {
            bagSlot.item = item;
            return true;
        }

        return false;
    }

    public void CopyTo(Array array, int index)
    {
        for (int i = index; i < array.Length; i++) {
            array.SetValue(m_bagSlots[i], i);
        }
    }

    // --- Helper Methods ---
    public BagSlot GetEmptyBagSlot()
    {
        foreach (BagSlot bagSlot in m_bagSlots) {
            if (bagSlot.IsEmpty()) {
                return bagSlot;
            }
        }

        return null;
    }
}
