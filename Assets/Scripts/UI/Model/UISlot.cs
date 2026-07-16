using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;


public abstract class UISlot<T, U> : UIBase
    where T : SlotBase<U>
    where U : INotifyPropertyChanged, IIcon, IComparable
{
    // --- Members ---
    // NOTE: To be set from outside (e.g. BagUI)
    protected T m_slot;
    public T slot
    {
        get => m_slot;
        set {
            if (!value.Equals(m_slot)) {
                UnsubscribeEvents();
                m_slot = value;
                SubscribeEvents();

                UpdateUI();
            }
        }
    }

    [SerializeField] protected Image m_image;


    // --- Methods ---
    protected virtual void Awake()
    {
        Debug.Assert(m_image != null, "Image component is not assigned in the inspector.");
    }

    protected virtual void OnEnable()
    {
        SubscribeEvents();
    }

    protected virtual void OnDisable()
    {
        UnsubscribeEvents();
    }

    public override void UpdateUI()
    {
        if (m_slot != null && !m_slot.IsEmpty()) {
            m_image.sprite = m_slot.icon;
            SetTransparent(m_image, false);
        }
        else {
            m_image.sprite = null;
            SetTransparent(m_image, true);
        }
    }


    // --- Event Handling ---
    protected virtual void SubscribeEvents()
    {
        if (m_slot != null) {
            m_slot.PropertyChanged += PropertyChangedHandler;
        }
    }

    protected virtual void UnsubscribeEvents()
    {
        if (m_slot != null) {
            m_slot.PropertyChanged -= PropertyChangedHandler;
        }
    }

    protected virtual void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
    {
        UpdateUI();
    }
}
