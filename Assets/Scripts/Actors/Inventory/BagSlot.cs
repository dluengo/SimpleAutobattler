using System.ComponentModel;
using System;
using UnityEngine;

[Serializable]
public class BagSlot : SlotBase<Item>
{
    public Item item
    {
        get => element;
        set => element = value;
    }
}