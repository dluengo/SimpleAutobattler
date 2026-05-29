using UnityEngine;

public abstract class AttackAction : ActorAction
{
    // --- Members ---
    [Header("--- Attack Settings ---")]
    public float baseDamage = 1f;

    protected DamageStat m_damageStat;

    // NOTE: The range area is the circle where the actor could start
    // attacking if there is an enemy.
    [SerializeField] bool enableSuggestedDistanceGizmo = true;
    public float suggestedDistance;


    // --- Methods ---
    protected override void Awake()
    {
        base.Awake();

        m_damageStat = GetComponent<DamageStat>();
    }

    protected float CalculateDamage()
    {
        // If the actor has a DamageStat use it to calculate the output damage.
        if (m_damageStat) {
            return (baseDamage + m_damageStat.currentValue) * m_damageStat.damageMultiplier;
        }
        else {
            return baseDamage;
        }
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
