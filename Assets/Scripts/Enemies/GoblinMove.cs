using UnityEngine;

[RequireComponent(typeof(EnemyController))]
public class GoblinMove : ActorMove
{
    // --- Members ---
    private EnemyController m_enemyController { get => m_actor as EnemyController; }


    // --- Methods ---
    protected override void Update()
    {
        base.Update();

        // Move in the direction of the player.
        if (m_enemyController.player != null) {
            Vector2 playerDir = m_enemyController.player.transform.position - transform.position;

            // Goblins just run towards the player.
            m_actor.move.moveDir = playerDir.normalized;
        }
        else {
            m_actor.move.moveDir = Vector2.zero;
        }
    }
}
