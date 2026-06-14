using UnityEngine;
using System;


[Serializable]
public abstract class Attribute : Stat
{
    [Header("--- Attribute Settings ---")]
    // NOTE: This is a trick to allow us to set the initial value through
    // inspector.
    [SerializeField] protected int m_attrValue;
    public int attrValue
    {
        // NOTE: Could there be problems with the cast?
        get => (int)value;
        set {
            this.value = value;
            m_attrValue = value;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        attrValue = m_attrValue;
    }

    protected override float CalculateStatValue()
    {
        return attrValue;
    }
}