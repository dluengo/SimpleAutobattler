using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    // --- Members ---
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private float explosionForce = 5f;
    [SerializeField] private LayerMask affectedLayers;

    /// <summary>
    /// Call this method to trigger the explosion effect at the current position.
    /// </summary>
    public void TriggerExplosion()
    {
        // Visual or sound effects can be triggered here

        // Find all colliders in the explosion radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, affectedLayers);
        foreach (var hit in hits)
        {
            // Example: apply force if the object has a Rigidbody2D
            Rigidbody2D rb = hit.attachedRigidbody;
            if (rb != null)
            {
                Vector2 direction = (rb.position - (Vector2)transform.position).normalized;
                rb.AddForce(direction * explosionForce, ForceMode2D.Impulse);
            }

            // You can add more logic here, e.g., baseDamage, status effects, etc.
        }
    }

    // Optional: visualize the explosion radius in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}