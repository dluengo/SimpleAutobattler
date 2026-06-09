using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ActorController))]
public class StatsController : MonoBehaviour, IEnumerable
{
    // -- Members ---
    [Header("--- Stats Controller Settings ---")]
    //public bool useHPStat = false;
    //public int baseHP;
    //public bool useVitalityStat = false;
    //public int vitality;

    protected List<Stat> m_statList;
    protected ActorController m_actor;


    // --- Methods ---
    private void Awake()
    {
        m_statList = new List<Stat>();
        m_actor = GetComponent<ActorController>();
        Debug.Assert(m_actor != null, "StatsController requires an ActorController component on the same GameObject.");

        // Look for all the stats components in the actor
        m_actor.gameObject.GetComponents<Stat>(m_statList);

        //Vitality vitalityStat = null;
        //if (useVitalityStat) {
        //    vitalityStat = new Vitality(vitality);
        //    m_statList.Add(vitalityStat);
        //}

        //if (useHPStat) {
        //    m_statList.Add(new HitPoints(baseHP, vitalityStat));
        //}
    }


    public IEnumerator GetEnumerator()
    {
        return m_statList.GetEnumerator();
    }

    public T GetStat<T>() where T : Stat
    {
        foreach (Stat stat in m_statList) {
            if (stat is T) {
                return stat as T;
            }
        }

        return null;
    }
}
