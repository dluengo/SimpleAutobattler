using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(ActorController))]
public class StatsController : IEnumerable
{
    // -- Members ---
    //[Header("--- Stats Controller Settings ---")]
    //public bool useHPStat = false;
    //public int baseHP;
    //public bool useVitalityStat = false;
    //public int vitality;

    protected List<Stat> m_statList;
    protected ActorController m_actor;


    // --- Methods ---
    //private void Awake()
    //{
    //    m_statList = new List<Stat>();
    //    m_actor = GetComponent<ActorController>();
    //    Debug.Assert(m_actor != null, "StatsController requires an ActorController component on the same GameObject.");

    //    // Look for all the stats components in the actor
    //    m_actor.gameObject.GetComponents<Stat>(m_statList);
    //}

    public StatsController(ActorController actor)
    {
        m_statList = new List<Stat>();
        m_actor = actor;
        Debug.Assert(m_actor != null, "StatsController requires an ActorController component on the same GameObject.");

        // Look for all the stats components in the actor
        m_actor.gameObject.GetComponents<Stat>(m_statList);
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
