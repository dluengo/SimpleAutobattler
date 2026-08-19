using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class AttributeSlotUI : UIBase
{
    // --- Members ---
    [HideInInspector] public Attribute attr;

    [Header("--- AttributeSlotUI Settings ---")]
    [SerializeField] Image m_icon;
    [SerializeField] TextMeshProUGUI m_valueText;

    public Button plusButton;
    public Button minusButton;

    public Sprite sprite
    {
        get => m_icon != null ? m_icon.sprite : null;
        set {
            if (m_icon != null) {
                m_icon.sprite = value;
            }
        }
    }


    // --- Methods ---
    // NOTE: To be called right after instantiating the prefab
    public void Init(
        Attribute attr,
        UnityEngine.Events.UnityAction plusBtnListener,
        UnityEngine.Events.UnityAction minusBtnListener)
    {
        this.attr = attr;

        //sprite = attr.statSO.icon;
        sprite = attr.icon;

        if (plusBtnListener != null) {
            plusButton.onClick.AddListener(plusBtnListener);
        }

        if (minusBtnListener != null) {
            minusButton.onClick.AddListener(minusBtnListener);
        }

        if (attr != null) {
            attr.OnValueChanged += UpdateUI;
        }
    }

    private void Awake()
    {
        //Debug.Assert(attr != null, "AttributeSlotUI: attr is not assigned.");
        Debug.Assert(m_icon != null, "AttributeSlotUI: m_icon is not assigned in the inspector.");
        Debug.Assert(m_valueText != null, "AttributeSlotUI: m_valueText is not assigned in the inspector.");
        Debug.Assert(plusButton != null, "AttributeSlotUI: plusButton is not assigned in the inspector.");
        Debug.Assert(minusButton != null, "AttributeSlotUI: minusButton is not assigned in the inspector.");
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

    protected override void Start()
    {
        base.Start();
    }

    public override void UpdateUI()
    {
        if (attr != null && m_valueText != null) {
            m_valueText.text = attr.currVal.ToString();
        }
    }
}