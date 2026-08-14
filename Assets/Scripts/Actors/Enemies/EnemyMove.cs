using UnityEngine;
using System;
using Unity.VisualScripting;

[RequireComponent(typeof(EnemyController))]
public class EnemyMove : ActorMove
{
    // --- Members ---
    [Header("--- Enemy Move Settings ---")]
    public float chaseRangeRadius = 5f;
    public float fleeRangeRadius = 2f;

    protected EnemyController m_enemyActor => m_actor as EnemyController;

    [SerializeField] bool enableChaseRangeGizmo = true;
    [SerializeField] bool enableFleeRangeGizmo = true;


    // --- Events ---
    public event Action<GameObject> OnPlayerInRange;


    // --- Methods ---
    // NOTE: Enemies move towards the enemyPlayer up to a certain distance.
    // They also run away from the enemyPlayer if they get too close.
    protected override void Update()
    {
        ActorController player = m_enemyActor != null ? m_enemyActor.enemyPlayer : null;
        if (player != null) {

            Vector2 directionToPlayer = player.transform.position - transform.position;
            float distanceToPlayer = directionToPlayer.magnitude;
            if (distanceToPlayer < fleeRangeRadius) {
                // Move away from the enemyPlayer if too close
                moveDir = -directionToPlayer.normalized;
                OnPlayerInRange?.Invoke(player.gameObject);
            }
            else if (distanceToPlayer > chaseRangeRadius) {
                // Move towards the enemyPlayer
                moveDir = directionToPlayer.normalized;
            }
            else {
                // Stop moving if within chase range but not too close
                moveDir = Vector2.zero;
                OnPlayerInRange?.Invoke(player.gameObject);
            }
        }

        base.Update();
    }


    // --- Gizmos ---
    protected virtual void OnDrawGizmosSelected()
    {
        // Draw chase range
        if (enableChaseRangeGizmo) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseRangeRadius);
        }

        // Draw flee range
        if (enableFleeRangeGizmo) {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, fleeRangeRadius);
        }
    }
}
