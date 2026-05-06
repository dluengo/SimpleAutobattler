using System.Collections;
using UnityEngine;

public class TNTGoblinController : EnemyController
{
    // --- Members ---
    [SerializeField] float m_range = 5;
    [SerializeField] float m_fleeRange = 3;
    [SerializeField] GameObject m_dynamitePrefab;
    [SerializeField] Transform projectileSpawnPoint;


    // --- Methods ---
    protected override void Update()
    {
        // Move towards the player if outside the range.
        if (player != null) {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            // If the attack is still on cooldown we move either towards the
            // player if they are further than range, or away from the player
            // if they are within flee range.
            //if (m_attackOnCooldown) {
                if (distanceToPlayer <= m_fleeRange) {
                    // Move away from the player.
                    move.moveDir = (transform.position - player.transform.position).normalized;
                }
                else if (distanceToPlayer <= m_range) {
                    // Stay still.
                    move.moveDir = Vector2.zero;
                } else { 
                    // Move towards the player
                    move.moveDir = (player.transform.position - transform.position).normalized;
                }
            //}
            //else {
            //    // Attack is not on cooldown, check if player is at range and attack if so.
            //    if (distanceToPlayer <= m_range) {
            //        Debug.Log($"Player is within range {distanceToPlayer}, starting attack.");
            //        StartAttack();

            //        // If the player is within flee range, move away from the player.
            //        if (distanceToPlayer <= m_fleeRange) {
            //            Debug.Log($"Player is within flee range {distanceToPlayer}, fleeing.");
            //            moveDir = (transform.position - player.transform.position).normalized;
            //        }
            //        else {
            //            // Otherwise, stay still.
            //            moveDir = Vector2.zero;
            //        }
            //    }
            //    else {
            //        // Move towards the player.
            //        moveDir = (player.transform.position - transform.position).normalized;
            //    }
            //}
        }
    }

    //protected override void OnAttackPerformedEventHandler()
    //{
    //    // Instantiate dynamite
    //    GameObject dynamiteGO = Instantiate(
    //        m_dynamitePrefab,
    //        projectileSpawnPoint.position,
    //        Quaternion.identity);

    //    if (dynamiteGO == null) {
    //        Debug.LogError("Failed to instantiate dynamite prefab!");
    //        return;
    //    }

    //    // Get the DynamiteController component and set its target.
    //    DynamiteController dynamite = dynamiteGO.GetComponent<DynamiteController>();
    //    if (dynamite == null) {
    //        Debug.LogError("Dynamite prefab does not have a DynamiteController component!");
    //        return;
    //    }

    //    dynamite.target = player.transform.position;
    //}


    // --- Gizmos ---
    private void OnDrawGizmosSelected()
    {
        // Draw a wire sphere to indicate the attack range.
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_range);
    }
}
