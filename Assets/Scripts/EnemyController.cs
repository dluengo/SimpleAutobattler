using UnityEngine;

public class EnemyController : ActorController
{
    // --- Methods ---
    protected void Update()
    {
        // Move towards the player
        PlayerController player = GameManager.Instance.Player;
        if (player != null) {
            direction = (player.transform.position - transform.position).normalized;
        }
    }


    // --- Collision Handling ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            Debug.Log("Enemy collided with Player!");
        }
    }
}
