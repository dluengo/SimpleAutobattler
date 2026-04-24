using UnityEngine;

public class StrengthStat : StatBase
{
    // --- Methods ---
    private void Awake()
    {
        statName = "Strength";
        maxValue = -1f;
        minValue = 0f;
        currentValue = initialValue;
    }
}
