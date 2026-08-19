using UnityEngine;
using System;


[Serializable]
public abstract class Attribute : Stat
{
    // --- Methods ---

    protected override int CalculateStatValue()
    {
        return currVal;
    }
}