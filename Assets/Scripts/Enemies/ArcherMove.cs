using UnityEngine;

public class ArcherMove : EnemyMove
{
    // --- Members ---
    [SerializeField] float m_range = 5f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawnPoint;


    // --- Methods ---
    protected override void Update()
    {
        // Archers move close to the player, and when they are in range
        // they attack.
        Vector2 dirPlayer = player.transform.position - transform.position;
        float distanceToPlayer = dirPlayer.magnitude;

        if (distanceToPlayer > m_range) {
            // Move towards player
            moveDir = dirPlayer;
        }
        else {
            // Attack player
            moveDir = Vector2.zero;
            //StartAttack();
        }
    }

    //protected override void OnAttackPerformedEventHandler()
    //{
    //    // Instantiate projectile
    //    if (m_projectilePrefab != null) {
    //        GameObject projectileGO =Instantiate(
    //            m_projectilePrefab,
    //            projectileSpawnPoint.position,
    //            Quaternion.identity);

    //        if (projectileGO == null) {
    //            Debug.LogError("Failed to instantiate projectile prefab!");
    //            return;
    //        }

    //        // Set projectile direction
    //        ProjectileController projectile = projectileGO.GetComponent<ProjectileController>();
    //        if (projectile == null) {
    //            Debug.LogError("Projectile prefab does not have a ProjectileController component!");
    //            return;
    //        }

    //        projectile.direction = (player.transform.position - projectileSpawnPoint.position).normalized;
    //        projectile.thrower = this;
    //    }
    //}

    // --- Gizmos ---
    private void OnDrawGizmosSelected()
    {
        // Draw attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_range);
    }
}
