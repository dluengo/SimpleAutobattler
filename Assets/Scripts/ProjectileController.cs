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
    private void Start()
    {
        StartCoroutine(DestroyAfterTime(expireTime));
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        // Move in the m_moveDir the projectile is facing (its up vector)
        transform.position += direction * projectileSpeed * Time.fixedDeltaTime;

        // Rotate to face the m_moveDir of movement
        if (direction != Vector3.zero) {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }


    // --- Collision Handling ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Projectile collided with {collision.gameObject.name}");

        // Check if the collided object's layer is in the enemyLayer mask
        if ((thrower.enemyLayer.value & (1 << collision.gameObject.layer)) != 0) {
            HPStat enemyHp = collision.gameObject.GetComponent<HPStat>();
            if (enemyHp != null) {
                enemyHp.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }
}
