using UnityEngine;

public class ShotMove : ProjectileMove
{
    protected override void Update()
    {
        // If there is a target, move towards it.
        Vector2 direction = Vector2.zero;
        if (projectile.target != Vector2.zero) {
            direction = (projectile.target - (Vector2)transform.position).normalized;
        }
        // If no target but there is direction, keep moving in that direction.
        else if (moveDir != Vector2.zero) {
            direction = moveDir.normalized;
        }

        moveDir = direction;
    }
}
