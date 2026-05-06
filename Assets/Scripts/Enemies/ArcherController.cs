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

        // Always look towards the player
        lookDir = (player.transform.position - transform.position).normalized;

        // When close enought to the player, perform the first action in the list
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= m_range && actions.Count > 0) {
            actions[0].StartAction();
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
