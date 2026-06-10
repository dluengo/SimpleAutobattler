using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(GoblinMove))]
public class GoblinController : EnemyController
{
    // --- Members ---
    //[SerializeField] int m_contactDamage = 1;

    ////public GoblinMove goblinMove { get => move as GoblinMove; }


    //// --- Methods ---
    //protected override void OnEnable()
    //{
    //    base.OnEnable();

    //    // Check if this enemy has Health component and subscribe to the
    //    // OnValueChanged event to trigger the death animation when health reaches 0.
    //    HitPoints hpStat = GetComponent<HitPoints>();
    //    if (hpStat != null) {
    //        hpStat.OnValueMin += OnDestroy;
    //    }
    //}

    //protected override void OnDisable()
    //{
    //    base.OnDisable();

    //    // Unsubscribe from the OnValueChanged event to prevent memory leaks.
    //    HitPoints hpStat = GetComponent<HitPoints>();
    //    if (hpStat != null) {
    //        hpStat.OnValueMin -= OnDestroy;
    //    }
    //}


    //// --- Collision Handling ---
    //protected void OnCollisionEnter2D(Collision2D collision)
    //{
    //   CheckPlayerAndDamage(collision);
    //}

    //protected void OnCollisionStay2D(Collision2D collision)
    //{
    //    CheckPlayerAndDamage(collision);
    //}

    //private void CheckPlayerAndDamage(Collision2D collision)
    //{
    //    // Check if the collision is with the enemyPlayer
    //    if (collision.gameObject.CompareTag("Player")) {

    //        // Apply baseDamage to the enemyPlayer
    //        HitPoints playerHp = collision.gameObject.GetComponent<HitPoints>();
    //        if (playerHp != null) {
    //            playerHp.ChangeHP(m_contactDamage);
    //        }
    //    }
    //}


    //// --- Events Handlers ---
    //private void OnDestroy()
    //{
    //    Destroy(gameObject);
    //}
}
