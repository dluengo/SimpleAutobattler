using UnityEngine;

public abstract class AttackAction : ActorAction
{
    // --- Members ---
    [Header("--- Attack Settings ---")]
    [SerializeField] protected float m_defaultDamage = 1f;

    // NOTE: The range area is the circle where the actor could start
    // attacking if there is an enemy.
    [SerializeField] bool enableSuggestedDistanceGizmo = true;
    public float suggestedDistance;

    protected Damage m_damageStat;


    // --- Methods ---
    protected override void Awake()
    {
        base.Awake();

        m_damageStat = GetComponent<Damage>();
    }

    protected float CalculateDamage()
    {
        // Damage is calculated using the damage stat if present (it should),
        // we return a default damage if damage stat is not present.
        return m_damageStat != null && m_damageStat.enabled ? m_damageStat.currVal : m_defaultDamage;
    }


    // --- Gizmos ---
    protected virtual void OnDrawGizmosSelected()
    {
        // Draw attack range area
        if (enableSuggestedDistanceGizmo) {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, suggestedDistance);
        }
    }
}
