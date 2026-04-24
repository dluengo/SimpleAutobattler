using UnityEngine;

public class GoblinController : EnemyController
{
    // --- Members ---
    [SerializeField] int m_contactDamage = 1;

    // --- Methods ---
    protected void Update()
    {
        // Move towards the player
        if (player != null) {
            direction = (player.transform.position - transform.position).normalized;
        }
    }

    // --- Collision Handling ---
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        //base.OnCollisionEnter2D(collision);

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
}
