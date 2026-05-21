using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(fileName = "ShotMoveSO", menuName = "Projectile/Shot Move Data")]
public class ShotMoveSO : ProjectileMoveSO
{
    public override ProjectileMove CreateMoveComponent(GameObject projectileGO)
    {
        ShotMove moveComponent = projectileGO.AddComponent<ShotMove>();
        if (moveComponent != null) {
            moveComponent.moveSpeed = moveSpeed;
            moveComponent.rotationSpeed = rotationSpeed;
        }

        return moveComponent;
    }
}
