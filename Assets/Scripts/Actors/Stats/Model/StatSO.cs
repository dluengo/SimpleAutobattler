using UnityEngine;

public abstract class StatSO : ScriptableObject
{
    public string statName;
    [TextArea] public string statDescription;
    public Sprite icon;
}