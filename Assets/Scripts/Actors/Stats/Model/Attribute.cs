using UnityEngine;
using System;


[Serializable]
public abstract class Attribute : Stat
{
    // NOTE: This class is just a wrapper around Stat. In our design,
    // Attributes are Stats, but it is helpful to "tag" some specific
    // stats as attributes for later use.
}