using UnityEngine;
using System;

public class TNTGoblinMove : EnemyMove
{
    //// --- Members ---
    //[Header("--- TNTGoblin Move Settings ---")]
    ////[SerializeField] float m_range = 5;
    ////public float range
    ////{
    ////    get => m_range;
    ////    protected set => m_range = value;
    ////}
    //[SerializeField] float m_fleeRange = 3;
    //public float fleeRange
    //{
    //    get => m_fleeRange;
    //    protected set => m_fleeRange = value;
    //}
    //private TNTGoblinController m_tntGoblinController { get => m_enemyActor as TNTGoblinController; }


    //// --- Methods ---
    //protected override void OnEnable()
    //{
    //    base.OnEnable();

    //    fleeRange = m_fleeRange;
    //    //range = m_range;
    //}

    ////protected override void Update()
    ////{
    ////    base.Update();

    //    //if (m_tntGoblinController != null && m_tntGoblinController.enemyPlayer != null) {
    //    //    Transform player = m_tntGoblinController.enemyPlayer.transform;
    //    //    float distanceToPlayer = Vector2.Distance(transform.position, player.position);

    //    //    // Too close, flee
    //    //    if (distanceToPlayer <= fleeRange) {
    //    //        moveDir = (transform.position - player.position).normalized;
    //    //    }
    //    //    // Within attack range, stay still.
    //    //    //else if (distanceToPlayer <= range) {
    //    //    else if (distanceToPlayer <= cha)
    //    //        moveDir = Vector2.zero;
    //    //    }
    //    //    // Too far, move towards enemyPlayer.
    //    //    else {
    //    //        moveDir = (player.position - transform.position).normalized;
    //    //    }
    //    //}
    ////}


    //// --- Gizmos ---
    ////protected override void OnDrawGizmosSelected()
    ////{
    ////    base.OnDrawGizmosSelected();

    ////    // Draw the attack range in the editor
    ////    Gizmos.color = Color.red;
    ////    if (m_tntGoblinController != null) {
    ////        Gizmos.DrawWireSphere(transform.position, range);
    ////    }
    ////}
}
