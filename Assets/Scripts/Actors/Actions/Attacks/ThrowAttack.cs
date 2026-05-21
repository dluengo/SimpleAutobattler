using UnityEngine;

public class ThrowAttack : RangedAttack
{
    // --- Members ---
    [Header("--- Throw Attack Settings ---")]
    public float throwArcHeight = 2f;


    // --- Methods ---
    protected override void PerformAction()
    {
        GameObject projectileGO = CreateProjectile();

        if (projectileGO != null) {
            ProjectileController projectile = projectileGO.GetComponent<ProjectileController>();

            // Set the target of the projectile
            if (projectile != null) {
                projectile.target = target;
                projectile.arcHeight = throwArcHeight;
            }
        }
    }

    protected override void ActionAnimEndHandler()
    {
        base.ActionAnimEndHandler();

        target = Vector2.zero;
    }
}
