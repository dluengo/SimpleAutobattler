using UnityEngine;

public abstract class AttackAction : ActorAction
{
    // --- Members ---
    [Header("--- Attack Settings ---")]
    public float damage = 1f;

    // NOTE: The range area is the circle where the actor could start
    // attacking if there is an enemy.
    [SerializeField] bool enableSuggestedDistanceGizmo = true;
    public float suggestedDistance;


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
