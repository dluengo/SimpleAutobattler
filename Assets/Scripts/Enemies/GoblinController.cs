using UnityEngine;

public class GoblinController : EnemyController
{
    protected void Update()
    {
        // Move towards the player
        if (player != null) {
            direction = (player.transform.position - transform.position).normalized;
        }
    }
}
