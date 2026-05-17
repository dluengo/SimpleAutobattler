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

        // Move in the target of the enemyPlayer.
        if (m_enemyController.enemyPlayer != null) {
            Vector2 playerDir = m_enemyController.enemyPlayer.transform.position - transform.position;

            // Goblins just run towards the enemyPlayer.
            m_actor.move.moveDir = playerDir.normalized;
        }
        else {
            m_actor.move.moveDir = Vector2.zero;
        }
    }
}
