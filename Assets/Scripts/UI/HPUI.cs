using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HPUI : StatUIBase
{
    // --- Members ---
    [Header("--- HP UI Settings ---")]
    [SerializeField] Slider hpSlider;
    [SerializeField] TextMeshProUGUI hpText;

    protected HPStat hpStat => stat as HPStat;


    // --- Methods ---
    protected override void Awake()
    {
        Debug.Assert(stat != null, "HPUI: No HPStat found in Player's statList.");
        Debug.Assert(hpSlider != null, "HPUI: hpSlider reference is not assigned.");
        Debug.Assert(hpText != null, "HPUI: hpText reference is not assigned.");
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (hpStat != null) {
            hpStat.OnMaxValueChanged += UpdateUI;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (hpStat != null) {
            hpStat.OnMaxValueChanged -= UpdateUI;
        }
    }

    public override void UpdateUI()
    {
        hpSlider.value = (float)hpStat.currentValue / hpStat.maxValue;
        hpText.text = $"{hpStat.currentValue} / {hpStat.maxValue}";
    }
}
