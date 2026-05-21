using UnityEngine;

public class ShotAttack : RangedAttack
{
    // --- Members ---


    // --- Methods ---
    protected override void PerformAction()
    {
        GameObject projectileGO = CreateProjectile();

        if (projectileGO != null) {
            ProjectileController projectile = projectileGO.GetComponent<ProjectileController>();

            // Set the direction of the projectile.
            if (projectile != null) {
                projectile.moveDir = actionDir;
            }
        }
    }
}
