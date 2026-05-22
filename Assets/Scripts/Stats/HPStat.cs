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
        // If the actor is invulnerable or damage is zero or less, do nothing.
        if (m_isInvulnerable || damage <= 0) {
            return;
        }

        // Damage is float but HP is int, we need to handle this.
        bool isTakingDamage = false;
        int newHP = Mathf.RoundToInt(currentValue - damage);
        if (newHP != currentValue) {
            isTakingDamage = true;
        }

        // Update the current value.
        currentValue = newHP > minValue ? newHP : minValue;

        //float newValue = currentValue - damage;
        //if (newValue != currentValue) {
        //    isTakingDamage = true;
        //}

        //int roundedNewValue = Mathf.RoundToInt(newValue);

        //currentValue = roundedNewValue > minValue ? roundedNewValue : minValue;

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
