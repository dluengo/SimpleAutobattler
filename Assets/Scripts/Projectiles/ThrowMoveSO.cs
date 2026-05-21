using UnityEngine;

[CreateAssetMenu(fileName = "ThrowMoveSO", menuName = "Projectile/Throw Move Data")]
public class ThrowMoveSO : ProjectileMoveSO
{
    public float arcHeight = 2f;

    public override ProjectileMove CreateMoveComponent(GameObject projectileGO)
    {
        ThrowMove moveComponent = projectileGO.AddComponent<ThrowMove>();
        if (moveComponent != null) {
            moveComponent.arcHeight = arcHeight;
        }

        return moveComponent;
    }
}
