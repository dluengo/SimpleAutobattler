using UnityEngine;

public class ThrowMove : ProjectileMove
{
    // --- Members ---
    [Header("--- Throw Projectile Settings ---")]
    [SerializeField] float arcHeight = 2f;

    private Vector2 m_startPosition;
    private float m_travelTime;
    private float m_elapsedTime = 0;
    private bool m_hasExploded = false;
    private Animator m_animator;
    private string m_explosionTriggerParamName = "End";


    // --- Methods ---
    protected override void Awake()
    {
        base.Awake();

        m_animator = GetComponent<Animator>();
        Debug.Assert(m_animator != null, "ThrowMove: Animator component is missing.");
    }

    protected virtual void Start()
    {
        m_startPosition = transform.position;
        m_travelTime = Vector2.Distance(m_startPosition, projectile.target) / projectileSpeed;
    }

    protected override void Update()
    {
        if (m_hasExploded) {
            return;
        }

        if (m_elapsedTime < m_travelTime) {
            m_elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(m_elapsedTime / m_travelTime);

            // Linear interpolation between start and target
            Vector2 currentPos = Vector2.Lerp(m_startPosition, projectile.target, t);

            // Parabolic arc: add vertical offset
            float height = arcHeight * 4 * (t - t * t); // Parabola: peak at t=0.5
            Vector3 pos = new Vector3(currentPos.x, currentPos.y + height, transform.position.z);

            //transform.position = pos;
            moveDir = (pos - transform.position).normalized;
        }
        else {
            // Reached target, trigger explosion
            moveDir = Vector2.zero;
            TriggerExplosion();
        }
    }

    private void TriggerExplosion()
    {
        m_hasExploded = true;
        if (m_animator != null) {
            m_animator.SetTrigger(m_explosionTriggerParamName);
        }
        // Optionally, destroy after animation ends (using Animation Event or Coroutine)
        Destroy(gameObject, 0.417f);
    }
}
