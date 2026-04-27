using UnityEngine;

public class DynamiteController : MonoBehaviour
{
    // --- Members ---
    public Vector2 target;
    [SerializeField] float m_speed = 5f;
    [SerializeField] float arcHeight = 2f;

    private Vector2 m_startPosition;
    private float m_travelTime;
    private float m_elapsedTime;
    private bool m_hasExploded = false;
    private Animator m_animator;
    private float m_explosionDuration = 0.417f; // Duration of the explosion animation in seconds

    // --- Methods ---
    private void Start()
    {
        m_startPosition = transform.position;
        m_travelTime = Vector2.Distance(m_startPosition, target) / m_speed;
        m_elapsedTime = 0f;
        m_animator = GetComponent<Animator>();
        Debug.Assert(m_animator != null, "DynamiteController: Animator component is missing.");
    }

    private void Update()
    {
        if (m_hasExploded) {
            return;
        }

        if (m_elapsedTime < m_travelTime) {
            m_elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(m_elapsedTime / m_travelTime);

            // Linear interpolation between start and target
            Vector2 currentPos = Vector2.Lerp(m_startPosition, target, t);

            // Parabolic arc: add vertical offset
            float height = arcHeight * 4 * (t - t * t); // Parabola: peak at t=0.5
            Vector3 pos = new Vector3(currentPos.x, currentPos.y + height, transform.position.z);

            transform.position = pos;
        }
        else {
            // Reached target, trigger explosion
            TriggerExplosion();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!m_hasExploded) {
            TriggerExplosion();
        }
    }

    private void TriggerExplosion()
    {
        m_hasExploded = true;
        if (m_animator != null) {
            m_animator.SetTrigger("Explode");
        }
        // Optionally, destroy after animation ends (using Animation Event or Coroutine)
        Destroy(gameObject, m_explosionDuration);
    }
}
