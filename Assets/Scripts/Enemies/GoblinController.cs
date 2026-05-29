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
    //    HPStat hpStat = GetComponent<HPStat>();
    //    if (hpStat != null) {
    //        hpStat.OnValueMin += OnDeath;
    //    }
    //}

    //protected override void OnDisable()
    //{
    //    base.OnDisable();

    //    // Unsubscribe from the OnValueChanged event to prevent memory leaks.
    //    HPStat hpStat = GetComponent<HPStat>();
    //    if (hpStat != null) {
    //        hpStat.OnValueMin -= OnDeath;
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
    //        HPStat playerHp = collision.gameObject.GetComponent<HPStat>();
    //        if (playerHp != null) {
    //            playerHp.TakeDamage(m_contactDamage);
    //        }
    //    }
    //}


    //// --- Events Handlers ---
    //private void OnDeath()
    //{
    //    Destroy(gameObject);
    //}
}
