using UnityEngine;

public class DynamiteController : MonoBehaviour
{
    // --- Members ---
    public Vector2 target;
    [SerializeField] float m_speed = 5f;
    [SerializeField] float arcHeight = 2f;

    private Vector2 startPosition;
    private float travelTime;
    private float elapsedTime;

    // --- Methods ---
    private void Start()
    {
        startPosition = transform.position;
        travelTime = Vector2.Distance(startPosition, target) / m_speed;
        elapsedTime = 0f;
    }

    private void Update()
    {
        if (elapsedTime < travelTime) {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / travelTime);

            // Linear interpolation between start and target
            Vector2 currentPos = Vector2.Lerp(startPosition, target, t);

            // Parabolic arc: add vertical offset
            float height = arcHeight * 4 * (t - t * t); // Parabola: peak at t=0.5
            Vector3 pos = new Vector3(currentPos.x, currentPos.y + height, transform.position.z);

            transform.position = pos;
        }
        else {
            // Reached target
            transform.position = new Vector3(target.x, target.y, transform.position.z);
            Destroy(gameObject);
        }
    }
}
