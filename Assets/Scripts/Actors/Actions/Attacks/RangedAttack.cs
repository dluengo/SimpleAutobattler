using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

public abstract class RangedAttack : AttackAction
{
    // --- Members ---
    [Header("--- Ranged Attack Settings ---")]
    [SerializeField] protected float m_projectileSpeed = 10f;
    [SerializeField] protected float m_projectileRotationSpeed = 360f;
    [SerializeField] protected GameObject m_projectilePrefab;
    [SerializeField] protected ProjectileMove m_projectileMoveCompPrefab;


    // --- Methods ---
    protected override void Awake()
    {
        base.Awake();

        Debug.Assert(m_projectileMoveCompPrefab != null, "RangedAttack: m_projectileMoveCompPrefab is not assigned.");
    }

    protected GameObject CreateProjectile()
    {
        // Create m_projectile and add the move component to it.
        GameObject projectileGO = Instantiate(
            m_projectilePrefab,
            transform.position,
            Quaternion.identity);

        if (projectileGO != null) {

            // Copy the component type from m_projectileMoveCompPrefab and add it to the projectileGO
            // NOTE: AddComponent() calls Awake().
            Type moveType = m_projectileMoveCompPrefab.GetType();
            ProjectileMove newMoveComponent = projectileGO.AddComponent(moveType) as ProjectileMove;

            ProjectileController projectile = projectileGO.GetComponent<ProjectileController>();
            if (projectile != null) {
                projectile.Init();
                projectile.thrower = m_actor;
                projectile.damage = damage;
                projectile.moveSpeed = m_projectileSpeed;
                projectile.rotationSpeed = m_projectileRotationSpeed;
                return projectileGO;
            }
        }

        return null;
    }
}
