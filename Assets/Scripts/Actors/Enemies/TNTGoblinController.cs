using System.Collections;
using UnityEngine;

public class TNTGoblinController : EnemyController
{
    // --- Members ---
    [Header("--- TNTGoblin Settings ---")]
    [SerializeField] GameObject m_dynamitePrefab;


    // --- Methods ---
    protected override void OnEnable()
    {
        base.OnEnable();

        // Subscribe to the OnPlayerInRange event in TNTGoblinMove
        if (m_move != null) {
            m_move.OnPlayerInRange += PlayerInRangeHandler;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        // Unsubscribe from the OnPlayerInRange event to prevent memory leaks.
        if (m_move != null) {
            m_move.OnPlayerInRange -= PlayerInRangeHandler;
        }
    }


    // --- Event Handlers ---
    private void PlayerInRangeHandler(GameObject playerGO)
    {
        // If the enemyPlayer is in range, throw a dynamite at the enemyPlayer's position.
        if (actions.Count > 0) {
            ThrowAttack throwAttackAction = actions[0] as ThrowAttack;

            // In theory this enemy has a ThrowAttack component attached to it,
            // if so we set the target to the enemyPlayer.
            if (throwAttackAction != null) {
                throwAttackAction.StartActionTarget(playerGO.transform.position);
            }
        }
    }
}
