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
    //[SerializeField] StatsController stats;


    //protected Attribute m_attr = null;


    // --- Methods ---
    protected override void Awake()
    {
        //Debug.Assert(attrs != null, "HPUI: attrs reference is not assigned.");
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

        if (stat != null) {
            stat.OnMaxHPChanged += UpdateUI;
            stat.OnBaseHPChanged += UpdateUI;
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
            stat.OnBaseHPChanged -= UpdateUI;
        }
    }

    public override void UpdateUI()
    {
        //if (m_attr != null) {
        //    hpSlider.value = (float)m_attr.value / m_attr.maxValue;
        //    hpText.text = $"{m_attr.value} / {m_attr.maxValue}";
        //}

        hpSlider.value = (float)stat.value / stat.maxHP;
        hpText.text = $"{stat.value} / {stat.maxHP}";
    }
}
