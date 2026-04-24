using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPUI : StatUIBase
{
    // --- Members ---
    [SerializeField] HPStat hpStat;
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI text;


    // --- Methods ---
    protected override void Awake()
    {
        Debug.Assert(hpStat != null, "HPStat is not assigned.");
        Debug.Assert(image != null, "Image component is not assigned.");
        Debug.Assert(text != null, "Text component is not assigned.");

        stat = hpStat;
        base.Awake();
    }

    protected override void UpdateUI()
    {
        text.text = hpStat.currentValue.ToString();
    }
}
