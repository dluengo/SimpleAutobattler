using System.Collections;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    // --- Members ---
    public Vector3 direction = Vector3.right;

    [SerializeField] float speed = 10f;
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
        // Move in the direction the projectile is facing (its up vector)
        transform.position += direction * speed * Time.fixedDeltaTime;

        // Rotate to face the direction of movement
        if (direction != Vector3.zero) {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}
