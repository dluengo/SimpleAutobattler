using System.Collections;
using UnityEngine;

public class HPStat : StatBase
{
    // --- Members ---
    //[SerializeField] string hpName = "Hit Points";
    //[SerializeField] float minHP = 0f;
    //[SerializeField] float maxHP;
    //[SerializeField] float initialHP;
    [SerializeField] float invulnerabilityDuration = 0.5f;

    private bool m_isInvulnerable = false;
    private SpriteRenderer m_spriteRenderer;

    // --- Methods ---
    protected override void Awake()
    {
        // NOTE: I don't like the fact that we StatBase needs this
        // data to be initialized.
        //statName = hpName;
        //maxValue = maxHP;
        //minValue = minHP;
        //initialValue = initialHP;
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        base.Awake();
    }

    public void TakeDamage(float damage)
    {
        if (m_isInvulnerable) {
            return;
        }

        float newValue = currentValue - damage;
        bool isTakingDamage = false;

        if (newValue != currentValue) {
            isTakingDamage = true;
        }

        currentValue = newValue > minValue ? newValue : minValue;

        if (isTakingDamage && invulnerabilityDuration > 0f) {
            StartCoroutine(InvulnerabilityCR());
        }
    }

    private IEnumerator InvulnerabilityCR()
    {
        m_isInvulnerable = true;

        // Make the sprite 50% transparent
        if (m_spriteRenderer != null)
        {
            Color c = m_spriteRenderer.color;
            m_spriteRenderer.color = new Color(c.r, c.g, c.b, 0.5f);
        }

        yield return new WaitForSeconds(invulnerabilityDuration);

        // Restore sprite alpha
        if (m_spriteRenderer != null)
        {
            Color c = m_spriteRenderer.color;
            m_spriteRenderer.color = new Color(c.r, c.g, c.b, 1f);
        }

        m_isInvulnerable = false;
    }
}
