using UnityEngine;

public class CollectableAnimation : MonoBehaviour
{
    [Header("Vertical Movement")]
    [SerializeField] private float amplitude = 0.5f;              // How high and low the item moves
    [SerializeField] private float speed = 1.0f;                  // Speed of the up/down movement
    [SerializeField] private bool startMoving = true;             // Start the movement immediately
    
    private Vector3 initialPosition;
    private float timeOffset;

    private void Start()
    {
        // Store initial position
        initialPosition = transform.position;
        
        // Add random offset so not all collectables move in sync
        timeOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    private void Update()
    {
        // Only move if startMoving is enabled
        if (!startMoving)
        {
            return;
        }
        
        // Calculate current time with offset
        float time = Time.time + timeOffset;
        
        // Smooth vertical movement using sine function
        float yMovement = Mathf.Sin(time * speed) * amplitude;
        
        // Apply movement to position (only Y axis)
        Vector3 newPosition = initialPosition + new Vector3(0, yMovement, 0);
        transform.position = newPosition;
    }
}
