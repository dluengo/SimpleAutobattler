using Unity.VisualScripting;
using UnityEngine;

public class GoblinController : EnemyController
{
    // --- Members ---
    [SerializeField] int m_contactDamage = 1;


    // --- Methods ---
    protected override void OnEnable()
    {
        base.OnEnable();

        // Check if this enemy has Health component and subscribe to the
        // OnValueChanged event to trigger the death animation when health reaches 0.
        HPStat hpStat = GetComponent<HPStat>();
        if (hpStat != null) {
            hpStat.OnValueMin += OnDeath;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        // Unsubscribe from the OnValueChanged event to prevent memory leaks.
        HPStat hpStat = GetComponent<HPStat>();
        if (hpStat != null) {
            hpStat.OnValueMin -= OnDeath;
        }
    }

    protected void Update()
    {
        // Move towards the player
        if (player != null) {
            m_moveDir = (player.transform.position - transform.position).normalized;
        }
    }

    // NOTE: Goblins don't attack, they just move towards the player.
    protected override void OnAttackPerformedEventHandler()
    {
        ;
    }


    // --- Collision Handling ---
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        // NOTE: Should we calle base.OnCollisionEnter2D(collision) here?

        // Check if the collision is with the player
        if (collision.gameObject.CompareTag("Player")) {
            // Apply damage to the player
            HPStat playerHp = collision.gameObject.GetComponent<HPStat>();
            if (playerHp != null) {
                playerHp.TakeDamage(m_contactDamage);
            }
        }
    }

    protected void OnCollisionStay2D(Collision2D collision)
    {
        // Check if the collision is with the player
        if (collision.gameObject.CompareTag("Player")) {
            // Apply damage to the player
            HPStat playerHp = collision.gameObject.GetComponent<HPStat>();
            if (playerHp != null) {
                playerHp.TakeDamage(m_contactDamage);
            }
        }
    }


    // --- Events Handlers ---
    private void OnDeath()
    {
        Destroy(gameObject);
    }
}
