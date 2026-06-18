using UnityEngine;

public class Helmet : Gear
{
    // --- Members ---
    public int defense { get; protected set; }

    protected HelmetSO m_helmetSO => m_gearSO as HelmetSO;


    // --- Methods ---
    public Helmet(HelmetSO helmetSO) : base(helmetSO)
    {
        defense = m_helmetSO.baseDefense;
    }
}
