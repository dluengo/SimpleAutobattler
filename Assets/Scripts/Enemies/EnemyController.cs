using UnityEngine;

public class EnemyController : ActorController
{
    // --- Members ---
    public ActorController enemyPlayer { get; protected set; }

    [Header("--- Enemy Settings ---")]
    public bool neutral = false;

    [SerializeField] float m_contactDamage = 1f;

    protected new EnemyMove m_move => base.m_move as EnemyMove;


    // --- Methods ---
    protected override void OnEnable()
    {
        base.OnEnable();

        // Check if this enemy has Health component and subscribe to the
        // OnValueChanged event to trigger the death animation when health reaches 0.
        if (m_hpStat != null) {
            m_hpStat.OnValueMin += OnDeadHandler;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        // Unsubscribe from the OnValueChanged event to prevent memory leaks.
        if (m_hpStat != null) {
            m_hpStat.OnValueMin -= OnDeadHandler;
        }
    }

    protected override void Start()
    {
        base.Start();

        enemyPlayer = GameManager.Instance.Player;

        // Initialize the chase radius of the EnemyMove component to the range
        // of the first attack action.
        if (m_move != null && actions.Count > 0) {
            AttackAction attackAction = actions[0] as AttackAction;
            if (attackAction != null && attackAction.suggestedDistance > 0) {
                m_move.chaseRangeRadius = attackAction.suggestedDistance;
            }
        }
    }

    // NOTE: By default enemiesInScene move towards the enemyPlayer until they are in range
    // then they attack continuously.
    protected override void Update()
    {
        base.Update();

        // If there is no move component, do nothing.
        if (m_move == null || !m_move.enabled) {
            return;
        }

        // If there is no player, stand still.
        if (enemyPlayer == null) {
            m_move.moveDir = Vector2.zero;
            return;
        }

        Vector2 directionToPlayer = enemyPlayer.transform.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (m_move != null && m_move.enabled) {

            // Player too close, flee.
            if (distanceToPlayer < m_move.fleeRangeRadius) {
                m_move.moveDir = -directionToPlayer.normalized;
            }
            // Player is far away, move towards them.
            else if (distanceToPlayer > m_move.chaseRangeRadius) {
                m_move.moveDir = directionToPlayer.normalized;
            }
            // Player is within action/attack range. Stop moving.
            else {
                m_move.moveDir = Vector2.zero;

                // Trigger the first action (presumably an attack).
                if (actions.Count > 0) {
                    actions[0].StartActionTarget(enemyPlayer.transform.position);
                }
            }
        }

        // NOTE: lookDir update is handled in ActorController. However, because we
        // may have changed the moveDir after base.Update(), we need to update
        // lookDir here in case it's needed.
        lookDir = (enemyPlayer.transform.position - transform.position).normalized;
    }


    // --- Collision Handling ---
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        CheckPlayerAndDamage(collision);
    }

    protected void OnCollisionStay2D(Collision2D collision)
    {
        CheckPlayerAndDamage(collision);
    }

    private void CheckPlayerAndDamage(Collision2D collision)
    {
        // Check if the collision is with my enemy
        if (collision.gameObject.layer == enemyLayer) {

            // Apply baseDamage to the enemyPlayer if it has an HPStat component.
            HPStat playerHp = collision.gameObject.GetComponent<HPStat>();
            if (playerHp != null && playerHp.enabled) {
                playerHp.TakeDamage(m_contactDamage);
            }   
        }
    }


    // --- Events Handlers ---
    private void OnDeadHandler()
    {
        EnemyManager.Instance.UnregisterEnemy(this);
        Die();
    }
}
