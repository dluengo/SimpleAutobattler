using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MeleeAttack : AttackAction
{
    // NOTE: The hitbox is an area that is used in PerformAction() to check
    // if there are enemies being hit by the attack.
    // NOTE: This hitbox is represented by a collider, however this collider
    // MUST be disabled. We need it to NOT generate collisions at all. We
    // just need its area. I found it easy using a collider for that matter.
    [Header("--- Melee Attack Settings ---")]
    [SerializeField] bool enableAttackHitBoxGizmo = true;


    // NOTE: We want it SerializeField so we can assign it in the inspector
    // so we know exactly which collider is the hitbox for this attack.
    [SerializeField] BoxCollider2D m_hitbox;
    //protected Vector2 m_hitboxOffset;
    //protected Vector2 m_hitboxSize;


    // --- Methods ---
    protected override void Awake()
    {
        base.Awake();

        Debug.Assert(m_hitbox != null, $"AttackAction {name} requires a Collider2D component for attack hitbox.");

        // NOTE!
        // We don't want the collider to be active and generating collisions
        // We need it to have an easy way to define the area of the hitbox for
        // when hit detection is performed.
        //m_hitboxOffset = m_hitbox.offset;
        //m_hitboxSize = m_hitbox.bounds.size;
        m_hitbox.enabled = false;
    }

    // NOTE: This method should be called by an Animation Event
    // configured in the attack animation clip.
    protected override void PerformAction()
    {
        // Check enemies within the attack hitbox.
        // NOTE: When the hitbox is disabled its size is 0, we need to enable
        // it to get the correct size for the OverlapBoxAll method.
        m_hitbox.enabled = true;
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(
            m_hitbox.bounds.center,
            m_hitbox.bounds.size,
            0f,
            m_actor.enemyLayer);
        m_hitbox.enabled = false;

        // Iterate through all the colliders we are hitting.
        foreach (Collider2D hitCollider in hitColliders) {

            // If the collider is an actor
            ActorController hitActor = hitCollider.GetComponent<ActorController>();
            if (hitActor != null) {

                // Check if it is my enemy
                if (m_actor.ActorIsEnemy(hitActor)) {

                    // Deal damage to the target actor.
                    hitActor.TakeDamage(damage);
                }
            }
        }
    }


    // --- Gizmos ---
    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        // Draw attack hitbox area
        if (enableAttackHitBoxGizmo) {

            m_hitbox.enabled = true;
            Vector3 center = m_hitbox.bounds.center;
            Vector3 size = m_hitbox.bounds.size;
            size *= 1.01f;
            m_hitbox.enabled = false;

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(center, size);
        }
    }
}
