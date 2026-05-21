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
    [SerializeField] protected ProjectileMoveSO m_projectileMoveSOPrefab;


    // --- Methods ---
    protected override void Awake()
    {
        base.Awake();
        
        Debug.Assert(m_projectilePrefab != null, "RangedAttack: m_projectilePrefab is not assigned.");
        Debug.Assert(m_projectileMoveSOPrefab != null, "RangedAttack: m_projectileMoveSOPrefab is not assigned.");
    }

    protected GameObject CreateProjectile()
    {
        // Create the new projectile and add the move component to it.
        GameObject projectileGO = Instantiate(
            m_projectilePrefab,
            transform.position,
            Quaternion.identity);

        if (projectileGO != null) {
            // Add the move component to the projectile.
            m_projectileMoveSOPrefab.CreateMoveComponent(projectileGO);

            // Initialize the projectile's controller.
            ProjectileController projectile = projectileGO.GetComponent<ProjectileController>();
            if (projectile != null) {
                projectile.Init(m_actor, damage, m_projectileSpeed, m_projectileRotationSpeed);
                return projectileGO;
            }
        }

        return null;
    }
}
