using UnityEngine;

[RequireComponent(typeof(ProjectileController))]
public abstract class ProjectileMove : MonoBehaviour
{
    // --- Members ---
    [HideInInspector] public Vector2 moveDir = Vector2.zero;

    [Header("--- Projectile Movement Settings ---")]
    public float projectileSpeed = 10f;

    protected ProjectileController projectile;


    // --- Methods ---
    protected virtual void Awake()
    {
        projectile = GetComponent<ProjectileController>();
        Debug.Assert(projectile != null, "ProjectileMove: ProjectileController component is missing.");
    }

    protected abstract void Update();
}
