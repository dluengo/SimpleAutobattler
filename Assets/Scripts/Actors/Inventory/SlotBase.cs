using System;
using System.ComponentModel;
using UnityEngine;

[Serializable]
public abstract class SlotBase<T> : INotifyPropertyChanged, IIcon where T : INotifyPropertyChanged, IIcon, IComparable
{
    protected T m_element;
    public T element
    {
        get => m_element;
        set {
            if (!value.Equals(m_element)) {
                UnsubscribeEvents();
                m_element = value;
                SubscribeEvents();

                // TODO: Debug, PropertyChanged is null here, however BagUI should've triggered
                // the subscription of every BagSlotUI...
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(element)));
            }
        }
    }
    public Sprite icon { get => m_element != null ? m_element.icon : null; }


    // --- Events ---
    public event PropertyChangedEventHandler PropertyChanged;


    // --- Methods ---
    public virtual bool AddElement(T element, bool dropCurrentItem)
    {
        if (this.element != null && this.element.Equals(element)
            || this.element == null && element == null) {
            Debug.LogWarning("Attempted to add an element that is already in the slot (or null).");
            return false;
        }

        if (this.element != null && dropCurrentItem) {
            if (!DropItem()) {
                Debug.LogWarning("Failed to drop the current item in the slot.");
                return false;
            }
        }

        this.element = element;
        return true;
    }

    public virtual bool IsEmpty()
    {
        return m_element == null;
    }

    public virtual bool DropItem()
    {
        if (m_element != null) {
            // TODO: EXTRA LOGIC TO ACTUALLY DROP AN ITEM IN THE GAME WORLD (e.g., instantiate a prefab, etc.)
            element = default;
            return true;
        }

        return false;
    }


    // --- Helper Methods ---
    protected virtual void SubscribeEvents()
    {
        if (m_element != null) {
            m_element.PropertyChanged += OnElementPropertyChanged;
        }
    }

    protected virtual void UnsubscribeEvents()
    {
        if (m_element != null) {
            m_element.PropertyChanged -= OnElementPropertyChanged;
        }
    }


    // -- Event Handling ---
    protected virtual void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        // Forward the event to subscribers of this slot
        PropertyChanged?.Invoke(this, e);
    }
}