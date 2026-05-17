using System.Collections;
using UnityEngine;

public class TNTGoblinController : EnemyController
{
    // --- Members ---
    [Header("--- TNTGoblin Settings ---")]
    [SerializeField] GameObject m_dynamitePrefab;

    protected TNTGoblinMove m_tntGoblinMove { get => move as TNTGoblinMove; }


    // --- Methods ---
    protected override void OnEnable()
    {
        base.OnEnable();

        // Subscribe to the OnPlayerInRange event in TNTGoblinMove
        if (m_tntGoblinMove != null) {
            m_tntGoblinMove.OnPlayerInRange += PlayerInRangeHandler;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        // Unsubscribe from the OnPlayerInRange event to prevent memory leaks.
        if (m_tntGoblinMove != null) {
            m_tntGoblinMove.OnPlayerInRange -= PlayerInRangeHandler;
        }
    }


    // --- Event Handlers ---
    private void PlayerInRangeHandler(GameObject playerGO)
    {
        Debug.Log("Player in range of TNTGoblin!");
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
