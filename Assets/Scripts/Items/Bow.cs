using UnityEngine;

public class Bow : Gear
{
    // --- Members ---
    public int damage => m_bowSO.damage;
    public int range => m_bowSO.range;

    protected BowSO m_bowSO => m_gearSO as BowSO;


    // --- Methods ---
    public Bow(BowSO bowSO) : base(bowSO) {}
}
