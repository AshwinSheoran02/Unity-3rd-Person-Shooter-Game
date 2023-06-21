using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    public float bounceSpeed = 1f;
    public float bounceHeight = 1f;

    private Vector3 startPosition;

    private void Start()
    {
        // Get the starting position of the arrow
        startPosition = transform.position;
    }

    private void Update()
    {
        // Calculate the new y position of the arrow using a sine wave
        float newY = startPosition.y + Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;

        // Set the new position of the arrow
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
