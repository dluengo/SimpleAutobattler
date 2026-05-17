using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ActorController))]
public class HPStat : StatBase
{
    // --- Members ---
    [Header("--- HP Settings ---")]
    [SerializeField] float invulnerabilityDuration = 0.5f;

    private bool m_isInvulnerable = false;
    private SpriteRenderer m_spriteRenderer;
    private string m_hpStatName = "Hit Points";


    // --- Methods ---
    protected void OnEnable()
    {
        // Subscribe to the OnValueMin event to trigger the death animation when health reaches 0.
        OnValueMin += OnHPZeroHandler;
    }

    protected void OnDisable() 
    {
        OnValueMin -= OnHPZeroHandler;
    }

    protected override void Start()
    {
        base.Start();

        m_statName = m_hpStatName;
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
        if (m_isInvulnerable) {
            return;
        }

        bool isTakingDamage = false;
        float newValue = currentValue - damage;
        if (newValue != currentValue) {
            isTakingDamage = true;
        }

        currentValue = newValue > minValue ? newValue : minValue;

        if (isTakingDamage && invulnerabilityDuration > 0f) {
            m_isInvulnerable = true;
            StartCoroutine(InvulnerabilityCR());
        }
    }

    private IEnumerator InvulnerabilityCR()
    {
        // Make the sprite 50% transparent
        if (m_spriteRenderer != null) {
            Color c = m_spriteRenderer.color;
            m_spriteRenderer.color = new Color(c.r, c.g, c.b, 0.5f);
        }

        yield return new WaitForSeconds(invulnerabilityDuration);

        // Restore sprite alpha
        if (m_spriteRenderer != null) {
            Color c = m_spriteRenderer.color;
            m_spriteRenderer.color = new Color(c.r, c.g, c.b, 1f);
        }

        m_isInvulnerable = false;
    }

    // --- Event Handlers ---
    private void OnHPZeroHandler()
    {
        //Debug.Log($"{m_enemyActor.gameObject.name} has reached 0 HP and will die.");
        m_actor.Die();
    }
}
