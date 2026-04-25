using System.Collections;
using UnityEngine;

public class TNTGoblinController : EnemyController
{
    // --- Members ---
    [SerializeField] float m_range = 5;
    [SerializeField] GameObject m_dynamitePrefab;


    // --- Methods ---
    protected void Update()
    {
        // Move towards the player if outside the range.
        if (player != null) {
            if (Vector2.Distance(transform.position, player.transform.position) <= m_range) {
                // Stop moving towards the player and throw dynamite.
                m_moveDir = Vector2.zero;

                StartAttack();
            }
            else {
                m_moveDir = (player.transform.position - transform.position).normalized;
            }
        }
    }

    protected override void OnAttackPerformedEventHandler()
    {
        // Instantiate dynamite
        GameObject dynamiteGO = Instantiate(
            m_dynamitePrefab,
            transform.position,
            Quaternion.identity);

        if (dynamiteGO == null) {
            Debug.LogError("Failed to instantiate dynamite prefab!");
            return;
        }

        // Get the DynamiteController component and set its target.
        DynamiteController dynamite = dynamiteGO.GetComponent<DynamiteController>();
        if (dynamite == null) {
            Debug.LogError("Dynamite prefab does not have a DynamiteController component!");
            return;
        }

        dynamite.target = player.transform.position;
    }


    // --- Gizmos ---
    private void OnDrawGizmosSelected()
    {
        // Draw a wire sphere to indicate the attack range.
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_range);
    }
}
