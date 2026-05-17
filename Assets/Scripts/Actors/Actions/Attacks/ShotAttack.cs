using UnityEngine;

public class ShotAttack : RangedAttack
{
    // --- Members ---
    protected ShotMove m_shotMoveCompPrefab => m_projectileMoveCompPrefab as ShotMove;


    // --- Methods ---
    protected override void PerformAction()
    {
        GameObject projectileGO = CreateProjectile();

        if (projectileGO != null && m_shotMoveCompPrefab != null) {

            // Set the direction of the projectile.
            ProjectileController projectile = projectileGO.GetComponent<ProjectileController>();
            if (projectile != null) {
                projectile.moveDir = actionDir;
            }
        }
    }
}
