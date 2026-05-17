using UnityEngine;

public class LancerMove : ActorMove
{
    //protected override void Update()
    //{
    //    // NOTE: Lancers move towards the enemyPlayer up to a range, then stay there.
    //    // If the enemyPlayer leaves the range lancers will chase again.

    //    PlayerController enemyPlayer = GameManager.Instance.Player;
    //    if (enemyPlayer == null) {
    //        return;
    //    }

    //    float distanceToPlayer = Vector2.Distance(transform.position, enemyPlayer.transform.position);

    //    // If playe is out of chase range move towards them.
    //    if (distanceToPlayer > chaseRangeRadius) {
    //        moveDir = (enemyPlayer.transform.position - transform.position).normalized;
    //    }
    //    // if enemyPlayer is in chase range stand still.
    //    else {
    //        moveDir = Vector2.zero;
    //    }

    //    base.Update();
    //}


    // --- Gizmos ---
    //private void OnDrawGizmosSelected()
    //{
    //    // Draw chase range radius.
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, chaseRangeRadius);
    //}
}
