using UnityEngine;

public abstract class StatSO : ScriptableObject
{
    public string statName;
    [TextArea] public string statDescription;
    public Sprite icon;
    public bool hasMinValue = false;
    public float minValue = 0f;
}