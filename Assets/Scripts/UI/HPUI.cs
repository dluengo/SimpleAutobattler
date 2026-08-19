using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HPUI : StatUI<HitPoints>
{
    // --- Members ---
    [Header("--- HP UI Settings ---")]
    [SerializeField] Slider hpSlider;
    [SerializeField] TextMeshProUGUI hpText;


    // --- Methods ---
    protected override void Awake()
    {
        Debug.Assert(stat != null, "HPUI: HitPoints reference is not assigned.");
        Debug.Assert(hpSlider != null, "HPUI: hpSlider reference is not assigned.");
        Debug.Assert(hpText != null, "HPUI: hpText reference is not assigned.");

        //if (attrs != null) {
        //    foreach (Attribute attr in attrs) {
        //        if (attr.attributeType == AttributeType.Vitality) {
        //            m_attr = attr;
        //            break;
        //        }
        //    }
        //}
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        //if (m_attr != null) {
        //    m_attr.OnMaxValueChanged += UpdateUI;
        //}

        //Debug.Log($"HPUI: Subscribing to OnValueChanged event in {stat.gameObject.name}");

        if (stat != null) {
            stat.OnMaxHPChanged += UpdateUI;
            stat.OnValueChanged += UpdateUI;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        //if (m_attr != null) {
        //    m_attr.OnMaxValueChanged -= UpdateUI;
        //}

        if (stat != null) {
            stat.OnMaxHPChanged -= UpdateUI;
            stat.OnValueChanged -= UpdateUI;
        }
    }

    public override void UpdateUI()
    {
        //if (m_attr != null) {
        //    hpSlider.value = (float)m_attr.value / m_attr.maxValue;
        //    hpText.text = $"{m_attr.value} / {m_attr.maxValue}";
        //}

        hpSlider.value = (float)stat.currVal / stat.maxHP;
        hpText.text = $"{stat.currVal} / {stat.maxHP}";
    }
}
