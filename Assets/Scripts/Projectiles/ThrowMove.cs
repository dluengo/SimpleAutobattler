using UnityEngine;
using System;

public class ThrowMove : ProjectileMove
{
    // --- Members ---
    [Header("--- Throw Projectile Settings ---")]
    public float arcHeight = 2f;

    [SerializeField] float m_travelTime = 0.5f;

    private Vector2 m_startPosition;
    private float m_elapsedTime = 0;


    // --- Methods ---
    protected override void Start()
    {
        base.Start();

        m_startPosition = transform.position;
        m_elapsedTime = 0;

        // Calculate travel time based on distance and move speed
        if (target.HasValue) {
            float distance = Vector2.Distance(m_startPosition, target.Value);
            m_travelTime = distance / moveSpeed;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (transform.position != m_projectile.target) {
            m_elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(m_elapsedTime / m_travelTime);

            // Linear interpolation between start and target
            Vector2 currentPos = Vector2.Lerp(m_startPosition, target.Value, t);

            // Parabolic arc: add vertical offset
            float height = arcHeight * 4 * (t - t * t); // Parabola: peak at t=0.5
            Vector3 pos = new Vector3(currentPos.x, currentPos.y + height, transform.position.z);

            moveDir = (pos - transform.position).normalized;

            // Rotate the projectile
            //transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
        else {
            // Reached target, trigger explosion
            moveDir = Vector2.zero;
            //OnTargetReachedInvoke();
        }
    }
}
