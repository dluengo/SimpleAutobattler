using UnityEngine;

public abstract class EnemyMove : ActorMove
{
    // --- Members ---
    protected PlayerController player;


    // --- Methods ---
    protected override void Start()
    {
        base.Start();

        player = GameManager.Instance.Player;
        Debug.Assert(player != null, "Player reference is null in EnemyMove!");
    }

    protected void OnDestroy()
    {
        EnemyManager.Instance.UnregisterEnemy(this);
    }


    // --- Collision Handling ---
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Enemy collided with {collision.gameObject.name}!");
    }
}
