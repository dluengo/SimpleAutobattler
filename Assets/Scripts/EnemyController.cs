using UnityEngine;

public abstract class EnemyController : ActorController
{
    // --- Members ---
    protected PlayerController player;


    // --- Methods ---
    protected void Start()
    {
        player = GameManager.Instance.Player;
        Debug.Assert(player != null, "Player reference is null in EnemyController!");
    }


    // --- Collision Handling ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            Debug.Log("Enemy collided with Player!");
        }
    }
}
