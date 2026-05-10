using UnityEngine;

public class ThrowAttackAction : ActorAction
{
    // --- Members ---
    // When target is set, the projectil will end when it reaches the target.
    // If target is zero, the projectile will keep moving until it expires or
    // hits something. Useful when you shoot projectiles and throw projectiles.
    [HideInInspector] public Vector2 target = Vector2.zero;

    [Header("--- Throw Attack Settings ---")]
    [SerializeField] protected GameObject m_projectilePrefab;


    // --- Methods ---
    protected override void PerformAction()
    {
        // Create projectile and set its target
        GameObject projectileGO = Instantiate(
            m_projectilePrefab,
            transform.position,
            Quaternion.identity);

        if (projectileGO != null) {
            ProjectileController projectile = projectileGO.GetComponent<ProjectileController>();
            if (projectile != null) {
                projectile.target = target;
                projectile.thrower = m_actor;
            }
            else {
                Debug.LogError("Projectile prefab does not have a ProjectileController component.");
            }
        }
    }

    protected override void ActionAnimEndHandler()
    {
        base.ActionAnimEndHandler();

        target = Vector2.zero;
    }
}
