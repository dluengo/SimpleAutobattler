using UnityEngine;

public class EnemyController : ActorController
{
    // --- Members ---
    public PlayerController enemyPlayer { get; protected set; }

    [Header("--- Enemy Settings ---")]
    [SerializeField] float m_contactDamage = 1f;

    protected EnemyMove enemyMove;


    // --- Methods ---
    protected override void Awake()
    {
        base.Awake();

        enemyMove = GetComponent<EnemyMove>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        // Check if this enemy has Health component and subscribe to the
        // OnValueChanged event to trigger the death animation when health reaches 0.
        HPStat hpStat = GetComponent<HPStat>();
        if (hpStat != null) {
            hpStat.OnValueMin += OnDeadHandler;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        // Unsubscribe from the OnValueChanged event to prevent memory leaks.
        HPStat hpStat = GetComponent<HPStat>();
        if (hpStat != null) {
            hpStat.OnValueMin -= OnDeadHandler;
        }
    }

    protected override void Start()
    {
        base.Start();

        enemyPlayer = GameManager.Instance.Player;

        // Initialize the chase radius of the EnemyMove component to the range
        // of the first attack action.
        if (enemyMove != null && actions.Count > 0) {
            AttackAction attackAction = actions[0] as AttackAction;
            if (attackAction != null && attackAction.suggestedDistance > 0) {
                enemyMove.chaseRangeRadius = attackAction.suggestedDistance;
            }
        }
    }

    // NOTE: By default enemies move towards the enemyPlayer until they are in range
    // then they attack continuously.
    protected override void Update()
    {
        base.Update();

        if (enemyPlayer == null) {
            return;
        }

        // Always look towards the enemyPlayer
        lookDir = (enemyPlayer.transform.position - transform.position).normalized;

        if (enemyMove != null) {

            // When close enought to the enemyPlayer, perform the first action in the list
            float distanceToPlayer = Vector2.Distance(transform.position, enemyPlayer.transform.position);
            if (distanceToPlayer <= enemyMove.chaseRangeRadius && actions.Count > 0) {
                //actions[0].StartActionDirection(lookDir);
                actions[0].StartActionTarget(enemyPlayer.transform.position);
            }
        }
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

            // Apply damage to the enemyPlayer
            HPStat playerHp = collision.gameObject.GetComponent<HPStat>();
            if (playerHp != null) {
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
