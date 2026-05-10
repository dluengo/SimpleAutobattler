using UnityEngine;

[RequireComponent(typeof(ArcherController))]
public class ArcherMove : ActorMove
{
    // --- Members ---
    private ArcherController m_archerController { get => m_actor as ArcherController; }

    // --- Methods ---
    protected override void Update()
    {
        // If we don't have a player, just stop moving.
        if (m_archerController.player == null) {
            m_actor.move.moveDir = Vector2.zero;
            return;
        }

        // Archers move within range of the player
        Vector2 dirPlayer = m_archerController.player.transform.position - transform.position;
        float distanceToPlayer = dirPlayer.magnitude;
        if (distanceToPlayer > m_archerController.range) {
            // Move towards player
            m_actor.move.moveDir = dirPlayer;
        }
        else {
            // Stop moving
            m_actor.move.moveDir = Vector2.zero;
        }

        base.Update();
    }
}
