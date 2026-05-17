using UnityEngine;

public class ThrowAttack : RangedAttack
{
    // --- Members ---
    [Header("--- Throw Attack Settings ---")]
    public float throwArcHeight = 2f;
    protected ThrowMove m_throwMoveCompPrefab => m_projectileMoveCompPrefab as ThrowMove;


    // --- Methods ---
    protected override void PerformAction()
    {
        GameObject projectileGO = CreateProjectile();

        if (projectileGO != null && m_throwMoveCompPrefab != null) {

            // Set the target of the projectile
            ProjectileController projectile = projectileGO.GetComponent<ProjectileController>();
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
