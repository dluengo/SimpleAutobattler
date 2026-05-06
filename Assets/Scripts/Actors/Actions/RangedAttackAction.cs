using UnityEngine;

public class RangedAttackAction : ActorAction
{
    // --- Members ---
    [Header("--- Ranged Attack Settings ---")]
    [SerializeField] protected GameObject m_projectilePrefab;


    // --- Methods ---
    protected override void PerformAction()
    {
        // Create projectile and set its direction towards the target
        GameObject projectileGO = Instantiate(
            m_projectilePrefab,
            transform.position,
            Quaternion.identity);

        if (projectileGO != null) {
            ProjectileController projectile = projectileGO.GetComponent<ProjectileController>();
            if (projectile != null) {
                projectile.direction = actionDir;
                projectile.thrower = m_actor;
            }
        }
    }
}
