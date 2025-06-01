using UnityEngine;

public class Lillypad : MonoBehaviour
{
    [Header("Water Movement")]
    [SerializeField] private float xAmplitude = 0.5f;          // Horizontal movement amplitude
    [SerializeField] private float zAmplitude = 0.3f;          // Depth movement amplitude
    [SerializeField] private float xSpeed = 1.0f;              // Horizontal movement speed
    [SerializeField] private float zSpeed = 0.8f;              // Depth movement speed
    [SerializeField] private float rotationAmplitude = 2.0f;   // Smooth rotation amplitude
    [SerializeField] private float rotationSpeed = 0.5f;       // Rotation speed
    
    private Vector3 initialPosition;
    private Vector3 initialRotation;
    private Vector3 previousPosition;
    private float timeOffset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Store initial position and rotation
        initialPosition = transform.position;
        initialRotation = transform.eulerAngles;
        previousPosition = initialPosition;
        
        // Add random offset so not all lillypads move in sync
        timeOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    // Update is called once per frame
    void Update()
    {
        // Store previous position
        previousPosition = transform.position;
        
        // Calculate current time with offset
        float time = Time.time + timeOffset;
        
        // Oscillatory movement in X and Z using sine functions
        float xMovement = Mathf.Sin(time * xSpeed) * xAmplitude;
        float zMovement = Mathf.Sin(time * zSpeed * 0.7f) * zAmplitude; // Slightly different speed for more natural movement
        
        // Apply movement to position
        Vector3 newPosition = initialPosition + new Vector3(xMovement, 0, zMovement);
        transform.position = newPosition;
        
        // Smooth rotation to simulate water swaying
        float yRotation = Mathf.Sin(time * rotationSpeed) * rotationAmplitude;
        float zRotation = Mathf.Cos(time * rotationSpeed * 0.8f) * rotationAmplitude * 0.5f;
        
        // Apply rotation
        Vector3 newRotation = initialRotation + new Vector3(0, yRotation, zRotation);
        transform.eulerAngles = newRotation;
    }

    private void OnCollisionStay(Collision collision)
    {
        // Check if the colliding object has a Rigidbody (probably the player)
        Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            // Calculate lillypad movement in this frame
            Vector3 lillypadMovement = transform.position - previousPosition;
            
            // Move player along with lillypad (position only, no rotation)
            playerRb.transform.position += lillypadMovement;
        }
    }
}
