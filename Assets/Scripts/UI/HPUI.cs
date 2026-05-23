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
        base.Awake();

        Debug.Assert(hpSlider != null, "HPUI: hpSlider reference is not assigned.");
        Debug.Assert(hpText != null, "HPUI: hpText reference is not assigned.");
    }

    protected override void Start()
    {
        base.Start();

        UpdateUI();
    }

    protected override void UpdateUI()
    {
        hpSlider.value = (float)hpStat.currentValue / hpStat.maxValue;
        hpText.text = $"{hpStat.currentValue} / {hpStat.maxValue}";
    }
}
