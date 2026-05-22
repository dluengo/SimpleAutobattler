using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HPUI : StatUIBase
{
    // --- Members ---
    //[SerializeField] HPStat hpStat;
    //[SerializeField] Sprite emptyHeartSprite;
    //[SerializeField] Sprite fullHeartSprite;
    //[SerializeField] GridLayoutGroup heartSlots;
    [Header("--- Stat UI Settings ---")]
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


        //// Populate the heartImages list and set the initial heart sprites.
        //for (int i = 0; i < heartSlots.transform.childCount; i++) {
        //    Image heartImage = heartSlots.transform.GetChild(i).GetComponent<Image>();
        //    //heartImages.Add(heartImage);
        //    heartImage.sprite = fullHeartSprite;
        //}
    }

    protected override void UpdateUI()
    {
        //float current = Mathf.Clamp(
        //    hpStat.currentValue,
        //    hpStat.minValue,
        //    heartSlots.transform.childCount);

        //// Update the transparency of the heart images based on the current HP value.
        //for (int i = 0; i < heartSlots.transform.childCount; i++)
        //{
        //    Image heartImage = heartSlots.transform.GetChild(i).GetComponent<Image>();
        //    Color color = heartImage.color;

        //    color.a = (i < current) ? 1f : 0f;
        //    heartImage.color = color;
        //}

        hpSlider.value = hpStat.currentValue / hpStat.maxValue;
        hpText.text = $"{hpStat.currentValue} / {hpStat.maxValue}";
    }
}
