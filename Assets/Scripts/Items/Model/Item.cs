using UnityEngine;
using System;
using System.ComponentModel;


// NOTE: Because this class is [Serializable] we can create objects through
// Inspector. Those Items objects will have their ItemSO property null.
[Serializable]
public class Item : IIcon, INotifyPropertyChanged, IComparable
{
    // --- Members ---
    // NOTE: To be set from inspector or at creation time.
    [Header("--- Item Settings ---")]
    [SerializeField] protected ItemSO m_itemSO;
    public ItemSO itemSO
    {
        get => m_itemSO;
        set
        {
            if (m_itemSO != value) {
                m_itemSO = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(itemSO)));
            }
        }
    }

    public string itemName => m_itemSO != null ? m_itemSO.itemName : null;
    public string description => m_itemSO != null ? m_itemSO.description : null;
    public Sprite icon => m_itemSO.icon;
    public AnimationClip idleClip => m_itemSO != null ? m_itemSO.idleClip : null;
    public AnimationClip pickedUpClip => m_itemSO != null ? m_itemSO.pickedUpClip : null;

    // Implementation of IIcon interface
    Sprite IIcon.icon { get => icon; }


    // --- Events ---
    public event PropertyChangedEventHandler PropertyChanged;

    
    // --- Methods ---
    protected Item(ItemSO itemSO)
    {
        if (itemSO == null) {
            throw new ArgumentNullException(nameof(itemSO), "ItemSO cannot be null.");
        }

        m_itemSO = itemSO;
    }


    public int CompareTo(Item other)
    {
        if (other == null) {
            return 1;
        }
        else if (this.itemSO == null && other.itemSO == null) {
            return 0;
        }
        else if (this.itemName == other.itemName) {
            return 0;
        }
        else if (this.itemName == null) {
            return -1;
        }
        else if (other.itemName == null) {
            return 1;
        }

        return 1;
    }

    public int CompareTo(object obj)
    {
        return CompareTo(obj as Item);
    }
}