using UnityEngine;

public class RangedAttackAction : ActorAction
{
    // --- Members ---
    [Header("--- Ranged Attack Settings ---")]
    [SerializeField] protected GameObject m_projectilePrefab;


    // --- Methods ---
    protected override void PerformAction()
    {
        // Create projectile and set its target towards the target
        GameObject projectileGO = Instantiate(
            m_projectilePrefab,
            transform.position,
            Quaternion.identity);

        if (projectileGO != null) {
            ProjectileController projectile = projectileGO.GetComponent<ProjectileController>();
            if (projectile != null) {
                projectile.target = Vector2.zero;
                projectile.moveDir = actionDir;
                projectile.thrower = m_actor;
            }
        }
    }
}
