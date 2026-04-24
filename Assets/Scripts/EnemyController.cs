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
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Enemy collided with {collision.gameObject.name}!");
    }
}
