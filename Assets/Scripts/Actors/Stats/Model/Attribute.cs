using UnityEngine;
using System;


[Serializable]
public abstract class Attribute : Stat
{
    // NOTE: This class is just a wrapper around Stat. In our design,
    // Attributes are Stats, but it is helpful to "tag" some specific
    // stats as attributes for later use.

    //[SerializeField] protected new AttributeSO m_statSO;
    //public new AttributeSO statSO
    //{
    //    get => m_statSO;
    //    protected set => m_statSO = value;
    //}

    [Header("--- Attribute Settings ---")]
    // NOTE: This is a trick to allow us to set the initial value through
    // inspector.
    [SerializeField] protected int m_attrValue;
    public int attrValue
    {
        get => value;
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

    protected override int CalculateStatValue()
    {
        return attrValue;
    }
}