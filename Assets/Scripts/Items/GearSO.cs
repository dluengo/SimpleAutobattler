using UnityEditor;
using UnityEngine;

public enum BodyPart
{
    Head,
    Torso,
    Legs,
    Feet,
    Hands
}

public abstract class GearSO : ItemSO
{
    // --- Members ---
    [Header("--- GearSO Settings ---")]
    public BodyPart bodyPart;
}
