using UnityEngine;

public abstract class EnemyController : ActorController
{
    // --- Members ---
    public PlayerController player { get; protected set; }

    [Header("--- Enemy Settings ---")]
    [SerializeField] float m_contactDamage = 1f;

    private string playerLayerName = "Player";


    // --- Methods ---
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
        player = GameManager.Instance.Player;
        enemyLayer = LayerMask.NameToLayer(playerLayerName);
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
        // Check if the collision is with the player
        if (collision.gameObject.CompareTag("Player")) {

            // Apply damage to the player
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
