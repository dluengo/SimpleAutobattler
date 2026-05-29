using System.Collections;
using UnityEngine;


public class HPStat : StatBase
{
    // --- Members ---
    [Header("--- HP Stat Settings ---")]
    [SerializeField] float invulnerabilityDuration = 0.5f;

    // TODO: Tint color is not working because of how the animation clips work.
    // Animation clips override the sprite of the gameobject, resetting the
    // color to the default color. Effectively overriding the tint color we set
    // For now we don't support it, although most likely this will come back to
    // bite us. But for now we leave some commented code.
    //[SerializeField] bool m_enableDamageTint = true;
    //public bool enableDamageTint
    //{
    //    get => m_enableDamageTint;
    //    set {
    //        // If disabling tint, reset sprite color to default.
    //        if (!value && m_spriteRenderer != null) {
    //            m_spriteRenderer.color = new Color(1, 1, 1, 1);
    //        }

    //        m_enableDamageTint = value;
    //        TintHandler();
    //    }
    //}

    private bool m_isInvulnerable = false;
    //private SpriteRenderer m_spriteRenderer;


    // --- Methods ---
    //protected override void Awake()
    //{
    //    base.Awake();

    //    m_spriteRenderer = GetComponent<SpriteRenderer>();
    //}

    protected void OnEnable()
    {
        // Subscribe to the OnValueMin event to trigger the death animation when health reaches 0.
        OnValueMin += OnHPZeroHandler;
        //OnValueChanged += TintHandler;
    }

    protected void OnDisable()
    {
        OnValueMin -= OnHPZeroHandler;
        //OnValueChanged -= TintHandler;
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

        // Trigger invulnerability if took damage and there is
        // any invulnerability duration
        if (isTakingDamage && invulnerabilityDuration > 0f) {
            m_isInvulnerable = true;
            StartCoroutine(InvulnerabilityCR());
        }
    }

    private IEnumerator InvulnerabilityCR()
    {
        // TODO: Same problem as with the tint. Because of how the animation clips work,
        // the sprite color is reset to default, overriding the alpha change we make here.
        //if (m_spriteRenderer != null) {
        //    Color c = m_spriteRenderer.color;
        //    m_spriteRenderer.color = new Color(c.r, c.g, c.b, 0.5f);
        //}

        yield return new WaitForSeconds(invulnerabilityDuration);

        // Restore sprite alpha
        //if (m_spriteRenderer != null) {
        //    Color c = m_spriteRenderer.color;
        //    m_spriteRenderer.color = new Color(c.r, c.g, c.b, 1f);
        //}

        m_isInvulnerable = false;
    }

    // --- Event Handlers ---
    private void OnHPZeroHandler()
    {
        m_actor.Die();
    }

    //private void TintHandler()
    //{
    //    Debug.Log($"HPStat: TintHandler called. currentValue={currentValue}, maxValue={maxValue}, enableDamageTint={enableDamageTint}");
    //    // Cache the SpriteRenderer reference if we haven't already.
    //    if (m_spriteRenderer == null) {
    //        return;
    //    }

    //    if (!enableDamageTint) {
    //        m_spriteRenderer.color = new Color(1, 1, 1, 1);
    //    }
    //    else {
    //        float healthPercent = Mathf.Clamp01((float)currentValue / maxValue);
    //        m_spriteRenderer.color = new Color(1, healthPercent, healthPercent, 1);
    //    }
    //}
}
