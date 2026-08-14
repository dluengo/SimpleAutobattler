using UnityEngine;

public class ArcherController : EnemyController
{
    // --- Members ---
    [SerializeField] float m_range = 5f;
    public float range {
        get => m_range;
        protected set => m_range = value;
    }


    // --- Methods ---
    protected override void OnEnable()
    {
        base.OnEnable();
        range = m_range;
    }

    protected override void Update()
    {
        base.Update();

        // If we don't have a enemyPlayer, just stop
        if (enemyPlayer == null) {
            return;
        }

        // Always look towards the enemyPlayer
        lookDir = (enemyPlayer.transform.position - transform.position).normalized;

        // When close enought to the enemyPlayer, perform the first action in the list
        float distanceToPlayer = Vector2.Distance(transform.position, enemyPlayer.transform.position);
        if (distanceToPlayer <= m_range && actions.Count > 0) {
            actions[0].StartActionDirection(lookDir);
        }
    }


    // --- Gizmos ---
    private void OnDrawGizmosSelected()
    {
        // Draw attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_range);
    }
}
