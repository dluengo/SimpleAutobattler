using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileMoveSO", menuName = "Projectile/Move Data")]
public abstract class ProjectileMoveSO : ScriptableObject
{
    // --- Members ---
    public float moveSpeed = 10f;
    public float rotationSpeed = 0f;


    // --- Methods ---
    public abstract ProjectileMove CreateMoveComponent(GameObject projectileGO);
}