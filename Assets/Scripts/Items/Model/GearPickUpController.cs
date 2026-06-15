using UnityEngine;

public class GearPickUpController : PickUpController
{
    // --- Members ---
    [Header("--- GearPickUp Settings ---")]
    [SerializeField] protected float floatAmplitude = 0.1f;
    [SerializeField] protected float floatFrequency = 5f;

    private Vector2 startPosition;


    // --- Methods ---
    protected override void Start()
    {
        base.Start();

        startPosition = transform.position;
    }

    protected void FixedUpdate()
    {
        // Make the Gear move in the Y axis with a sine wave to make it float.
        float newY = startPosition.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;

        // Gear don't have rigidbody.
        transform.position = new Vector2(startPosition.x, newY);
    }
}
