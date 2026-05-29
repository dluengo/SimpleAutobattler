using UnityEngine;

public class ShotAttack : RangedAttack
{
    // --- Members ---
    [Header("--- Shot Attack Settings ---")]
    // NOTE: When shooting while moving, we give the projectiles a bit
    // of deviation from the action direction, based on the actor's velocity.
    [SerializeField] protected float inheritVelocityFactor = 0.5f;


    // --- Methods ---
    protected override void PerformAction()
    {
        GameObject projectileGO = CreateProjectile();

        if (projectileGO != null) {
            ProjectileController projectile = projectileGO.GetComponent<ProjectileController>();

            if (projectile != null) {
                // Calculate the perpendicular direction to the actionDir
                Vector2 perp = new Vector2(-actionDir.y, actionDir.x);

                // Get the actor's velocity (assuming you have access to it)
                Vector2 actorVelocity = m_move != null ? m_move.moveDir * m_move.moveSpeed : Vector2.zero;

                // Project the actor's velocity onto the perpendicular direction
                float perpComponent = Vector2.Dot(actorVelocity, perp);
                Vector2 inherited = perp * (perpComponent * inheritVelocityFactor);

                // Set the projectile's moveDir with the inherited perpendicular velocity
                projectile.moveDir = (actionDir.normalized * projectile.moveSpeed + inherited).normalized;
            }
        }
    }
}
