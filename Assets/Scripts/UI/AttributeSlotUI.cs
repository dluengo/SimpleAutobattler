using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class AttributeSlotUI : UIBase
{
    // --- Members ---
    [HideInInspector] public Attribute attr;
    public Sprite sprite
    {
        get => m_icon != null ? m_icon.sprite : null;
        set {
            if (m_icon != null) {
                m_icon.sprite = value;
            }
        }
    }

    [Header("--- AttributeSlotUI Settings ---")]
    [SerializeField] Image m_icon;
    [SerializeField] TextMeshProUGUI m_valueText;
    [SerializeField] Slider m_slider;


    // --- Methods ---
    private void Awake()
    {
        Debug.Assert(m_icon != null, "AttributeSlotUI: m_icon is not assigned in the inspector.");
        Debug.Assert(m_valueText != null, "AttributeSlotUI: m_valueText is not assigned in the inspector.");
        Debug.Assert(m_slider != null, "AttributeSlotUI: m_slider is not assigned in the inspector.");
    }

    private void OnEnable()
    {
        if (attr != null) {
            attr.OnValueChanged += UpdateUI;
        }
    }
 
    private void OnDisable()
    {
        if (attr != null) {
            attr.OnValueChanged -= UpdateUI;
        }
    }

    public override void UpdateUI()
    {
        if (attr != null && m_valueText != null && m_slider != null) {
            m_valueText.text = attr.value.ToString();
            m_slider.value = attr.value;
        }
    }

    protected virtual void UpdateUI(int oldValue, int newValue)
    {
        UpdateUI();
    }
}