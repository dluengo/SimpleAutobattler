using System.Collections;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    // --- Members ---
    public float damage = 1f;

    [HideInInspector]
    public ActorController thrower;

    [HideInInspector]
    public Vector3 direction = Vector3.right;

    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float expireTime = 5f;


    // --- Methods ---
    //private void Awake()
    //{
    //    // Set projectile layer to match thrower's layer
    //    if (thrower != null) {
    //        gameObject.layer = thrower.gameObject.layer;
    //    } else {
    //        Debug.LogError("Projectile has no thrower assigned in Awake!");
    //    }
    //}

    private void Start()
    {
        if (thrower != null) {
            gameObject.layer = thrower.gameObject.layer;
        } else {
            Debug.LogError("Projectile has no thrower assigned in Start!");
        }
        StartCoroutine(DestroyAfterTime(expireTime));
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        // Move in the direction the projectile is facing
        transform.position += direction * projectileSpeed * Time.fixedDeltaTime;

        // Rotate to face the direction of movement
        if (direction != Vector3.zero) {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }


    // --- Collision Handling ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Projectile collided with {collision.gameObject.name}");

        if (thrower == null) {
            Debug.LogError("Projectile has no thrower assigned!");
            Destroy(gameObject);
            return;
        }

        // Check if the collided object is an ActorController
        ActorController hitActor = collision.gameObject.GetComponent<ActorController>();
        if (hitActor == null) {
            Debug.Log("Projectile hit a non-actor object, destroying projectile.");
            Destroy(gameObject);
            return;
        }

        bool throwerIsPlayer = thrower.CompareTag("Player");
        bool hitIsPlayer = hitActor.CompareTag("Player");
        bool throwerIsEnemy = thrower.CompareTag("Enemy");
        bool hitIsEnemy = hitActor.CompareTag("Enemy");

        // Player projectiles should not hit the player, only enemies
        if (throwerIsPlayer && hitIsPlayer) {
            // Ignore collision with self
            return;
        }

        // Enemy projectiles should not hit other enemies
        if (throwerIsEnemy && hitIsEnemy) {
            // Ignore collision with other enemies
            return;
        }

        // Apply damage if valid target
        HPStat hp = collision.gameObject.GetComponent<HPStat>();
        if (hp != null) {
            hp.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
