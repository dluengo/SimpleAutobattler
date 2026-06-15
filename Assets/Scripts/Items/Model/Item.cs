using UnityEngine;
using System;


[Serializable]
public class Item
{
    // --- Members ---
    [Header("--- Item Settings ---")]
    [SerializeField] protected ItemSO m_itemSO;
    public ItemSO itemSO => m_itemSO;

    public string itemName => m_itemSO.itemName;
    public string description => m_itemSO.description;
    public Sprite icon => m_itemSO.icon;
    public AnimationClip idleClip => m_itemSO.idleClip;
    public AnimationClip pickedUpClip => m_itemSO.pickedUpClip;


    // --- Methods ---
    public Item(ItemSO itemSO)
    {
        m_itemSO = itemSO;
    }
}