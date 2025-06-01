using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lillypad : MonoBehaviour
{
    [Header("Water Movement")]
    [SerializeField] private float xAmplitude = 0.5f;          // Horizontal movement amplitude
    [SerializeField] private float zAmplitude = 0.3f;          // Depth movement amplitude
    [SerializeField] private float xSpeed = 1.0f;              // Horizontal movement speed
    [SerializeField] private float zSpeed = 0.8f;              // Depth movement speed
    [SerializeField] private float rotationAmplitude = 2.0f;   // Smooth rotation amplitude
    [SerializeField] private float rotationSpeed = 0.5f;       // Rotation speed
    
    [Header("Waypoint Control")]
    [SerializeField] private bool isControlledByWaypoints = false;
    
    // Waypoint-controlled movement variables
    private List<Transform> waypointPath = new List<Transform>();
    private int currentWaypointIndex = 0;
    private bool isMovingToWaypoint = false;
    private float waypointMovementSpeed = 0f;
    private float sinkSpeed = 2f;
    private float sinkDepth = 3f;
    private float disappearDelay = 1f;
    private bool isSinking = false;
    private Vector3 initialSinkPosition;
    private Vector3 waypointMovementThisFrame = Vector3.zero;
    
    // Water movement variables
    private Vector3 initialPosition;
    private Vector3 initialRotation;
    private Vector3 previousPosition;
    private Vector3 waterMovementThisFrame = Vector3.zero;
    private float timeOffset;
    
    // Events
    public System.Action<Lillypad> OnLillypadDestroyed;

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
        
        // Reset movement vectors
        waterMovementThisFrame = Vector3.zero;
        waypointMovementThisFrame = Vector3.zero;
        
        // Declare newPosition once at the start
        Vector3 newPosition;
        
        // If sinking, only handle sinking movement
        if (isSinking)
        {
            Vector3 sinkMovement = Vector3.down * sinkSpeed * Time.deltaTime;
            newPosition = transform.position + sinkMovement;
            
            if (newPosition.y <= initialSinkPosition.y - sinkDepth)
            {
                OnLillypadDestroyed?.Invoke(this);
                Destroy(gameObject);
                return;
            }
            
            transform.position = newPosition;
            return;
        }
        
        // Calculate current time with offset
        float time = Time.time + timeOffset;
        
        // Water movement (only active if not controlled by waypoints or if waypoint movement finished)
        if (!isControlledByWaypoints || !isMovingToWaypoint)
        {
            float xMovement = Mathf.Sin(time * xSpeed) * xAmplitude;
            float zMovement = Mathf.Sin(time * zSpeed * 0.7f) * zAmplitude;
            Vector3 targetWaterPosition = initialPosition + new Vector3(xMovement, 0, zMovement);
            waterMovementThisFrame = targetWaterPosition - transform.position;
        }
        
        // Start with water movement
        newPosition = transform.position + waterMovementThisFrame;
        
        // Add waypoint movement if controlled by waypoints
        if (isControlledByWaypoints && isMovingToWaypoint)
        {
            Vector3 waypointMovement = MoveTowardsCurrentWaypoint();
            waypointMovementThisFrame = waypointMovement;
            newPosition += waypointMovement;
            
            // Update initial position so water movement is relative to current waypoint position
            initialPosition += waypointMovement;
        }
        
        transform.position = newPosition;
        
        // Smooth rotation to simulate water swaying (only if not sinking)
        float yRotation = Mathf.Sin(time * rotationSpeed) * rotationAmplitude;
        float zRotation = Mathf.Cos(time * rotationSpeed * 0.8f) * rotationAmplitude * 0.5f;
        
        // Apply rotation
        Vector3 newRotation = initialRotation + new Vector3(0, yRotation, zRotation);
        transform.eulerAngles = newRotation;
    }

    private Vector3 MoveTowardsCurrentWaypoint()
    {
        if (currentWaypointIndex >= waypointPath.Count)
        {
            return Vector3.zero;
        }

        Transform targetWaypoint = waypointPath[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        Vector3 movement = direction * waypointMovementSpeed * Time.deltaTime;
        
        // Check if we're close enough to the waypoint
        float distanceToWaypoint = Vector3.Distance(transform.position, targetWaypoint.position);
        
        // Use a more generous distance check and also check if we would overshoot
        bool reachedWaypoint = distanceToWaypoint <= 0.1f || movement.magnitude >= distanceToWaypoint;
        
        if (reachedWaypoint)
        {
            // Snap to waypoint position
            transform.position = targetWaypoint.position;
            initialPosition = targetWaypoint.position; // Update water movement center
            
            // Move to next waypoint
            currentWaypointIndex++;
            
            if (currentWaypointIndex >= waypointPath.Count)
            {
                ReachedFinalWaypoint();
                return Vector3.zero;
            }
            
            // Calculate movement to next waypoint
            if (currentWaypointIndex < waypointPath.Count)
            {
                Transform nextWaypoint = waypointPath[currentWaypointIndex];
                Vector3 nextDirection = (nextWaypoint.position - transform.position).normalized;
                return nextDirection * waypointMovementSpeed * Time.deltaTime;
            }
        }
        
        return movement;
    }

    private void ReachedFinalWaypoint()
    {
        isControlledByWaypoints = false;
        isMovingToWaypoint = false;
        initialSinkPosition = transform.position;
        initialPosition = transform.position;
        StartCoroutine(StartSinkingAfterDelay());
    }

    private IEnumerator StartSinkingAfterDelay()
    {
        yield return new WaitForSeconds(disappearDelay);
        isSinking = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        // Check if the colliding object has a Rigidbody (probably the player)
        Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            // Calculate total lillypad movement in this frame (water + waypoint movement)
            Vector3 totalLillypadMovement = waterMovementThisFrame + waypointMovementThisFrame;
            
            // Move player along with lillypad (position only, no rotation)
            playerRb.transform.position += totalLillypadMovement;
        }
    }

    // Public methods for waypoint control
    public void SetWaypointControl(bool controlled, List<Transform> path, float movementSpeed, float sSpeed, float sDepth, float delay)
    {
        isControlledByWaypoints = controlled;
        
        if (controlled && path != null && path.Count > 0)
        {
            waypointPath = new List<Transform>(path);
            waypointMovementSpeed = movementSpeed;
            sinkSpeed = sSpeed;
            sinkDepth = sDepth;
            disappearDelay = delay;
            currentWaypointIndex = 1;
            isMovingToWaypoint = true;
        }
        else
        {
            isMovingToWaypoint = false;
            waypointPath.Clear();
        }
    }
}
