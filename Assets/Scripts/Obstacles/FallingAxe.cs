using UnityEngine;

public class FallingAxe : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float targetRotation = 90f;           // Target rotation in degrees
    [SerializeField] private float movementDuration = 1f;          // Duration of the axe movement
    [SerializeField] private float preMovementPause = 2f;          // Pause before falling (when axe is up)
    [SerializeField] private float postMovementPause = 3f;         // Pause after falling (when axe is down)
    [SerializeField] private bool infiniteRotation = false;        // If true, rotates continuously without pauses
    
    [Header("Start Settings")]
    [SerializeField] private bool startMoving = true;             // Start the movement cycle immediately

    private float currentTimer = 0f;
    private Quaternion startRotation;
    private bool isMoving = false;
    private bool isAxeUp = true;                                   // True when axe is in starting position, false when rotated
    private float rotationSpeed;
    private float currentRotation = 0f;                            // Current rotation progress in degrees
    private bool isFullRotation = false;                           // True if targetRotation is multiple of 360
    private bool wasMovingLastFrame = false;                       // Track if movement was active last frame
    
    // Cache values to detect changes during runtime
    private float lastTargetRotation;
    private float lastMovementDuration;
    private bool lastStartMoving;

    private void Start()
    {
        startRotation = transform.rotation;
        UpdateRotationSettings();
        lastStartMoving = startMoving;
        
        InitializeMovement();
    }

    private void InitializeMovement()
    {
        if (startMoving)
        {
            if (infiniteRotation)
            {
                isMoving = true;
            }
            else
            {
                // Start in pre-movement pause (axe is up)
                isAxeUp = true;
                isMoving = false;
                currentTimer = 0f;
            }
            wasMovingLastFrame = true;
        }
        else
        {
            // Stop all movement
            isMoving = false;
            isAxeUp = true;
            currentTimer = 0f;
            currentRotation = 0f;
            transform.rotation = startRotation; // Reset to initial position
            wasMovingLastFrame = false;
        }
    }

    private void UpdateRotationSettings()
    {
        rotationSpeed = Mathf.Abs(targetRotation) / movementDuration; // degrees per second
        isFullRotation = Mathf.Abs(targetRotation) % 360f == 0f && targetRotation != 0f; // Check if it's a multiple of 360
        lastTargetRotation = targetRotation;
        lastMovementDuration = movementDuration;
    }

    private void Update()
    {
        // Check if settings changed during runtime and update accordingly
        if (lastTargetRotation != targetRotation || lastMovementDuration != movementDuration)
        {
            UpdateRotationSettings();
        }
        
        // Check if startMoving changed during runtime
        if (lastStartMoving != startMoving)
        {
            lastStartMoving = startMoving;
            InitializeMovement();
        }

        // Only process movement if startMoving is true
        if (!startMoving)
        {
            return;
        }

        if (infiniteRotation)
        {
            // Continuous rotation without pauses
            float rotationThisFrame = rotationSpeed * Time.deltaTime;
            
            if (isFullRotation)
            {
                // For 360° rotations, keep rotating in the same direction
                currentRotation += rotationThisFrame;
                if (currentRotation >= Mathf.Abs(targetRotation))
                {
                    currentRotation = 0f; // Reset to start a new full rotation
                }
            }
            else
            {
                // Normal back and forth movement
                if (isAxeUp)
                {
                    // Moving towards target rotation
                    currentRotation += rotationThisFrame;
                    if (currentRotation >= Mathf.Abs(targetRotation))
                    {
                        currentRotation = Mathf.Abs(targetRotation);
                        isAxeUp = false;
                    }
                }
                else
                {
                    // Moving back to start
                    currentRotation -= rotationThisFrame;
                    if (currentRotation <= 0f)
                    {
                        currentRotation = 0f;
                        isAxeUp = true;
                    }
                }
            }
            
            // Apply rotation using the sign of targetRotation for direction
            float actualRotation = currentRotation * Mathf.Sign(targetRotation);
            transform.rotation = startRotation * Quaternion.Euler(0, 0, actualRotation);
        }
        else
        {
            // State-based movement with pauses
            if (!isMoving)
            {
                currentTimer += Time.deltaTime;
                
                if (isAxeUp)
                {
                    // Axe is up - wait for pre-movement pause before falling
                    if (currentTimer >= preMovementPause)
                    {
                        isMoving = true;
                        currentTimer = 0f;
                    }
                }
                else
                {
                    // Axe is down - wait for post-movement pause before going back up
                    if (currentTimer >= postMovementPause)
                    {
                        isMoving = true;
                        currentTimer = 0f;
                    }
                }
            }
            else
            {
                // Currently moving
                currentTimer += Time.deltaTime;
                float rotationThisFrame = rotationSpeed * Time.deltaTime;
                
                if (isFullRotation)
                {
                    // For 360° rotations, keep rotating in the same direction
                    currentRotation += rotationThisFrame;
                    if (currentRotation >= Mathf.Abs(targetRotation) || currentTimer >= movementDuration)
                    {
                        currentRotation = 0f; // Reset to start a new full rotation
                        isMoving = false;
                        isAxeUp = true; // Always return to "up" state for next cycle
                        currentTimer = 0f;
                    }
                }
                else
                {
                    // Normal back and forth movement
                    if (isAxeUp)
                    {
                        // Moving towards target rotation
                        currentRotation += rotationThisFrame;
                        if (currentRotation >= Mathf.Abs(targetRotation) || currentTimer >= movementDuration)
                        {
                            currentRotation = Mathf.Abs(targetRotation);
                            isMoving = false;
                            isAxeUp = false;
                            currentTimer = 0f;
                        }
                    }
                    else
                    {
                        // Moving back to start
                        currentRotation -= rotationThisFrame;
                        if (currentRotation <= 0f || currentTimer >= movementDuration)
                        {
                            currentRotation = 0f;
                            isMoving = false;
                            isAxeUp = true;
                            currentTimer = 0f;
                        }
                    }
                }
                
                // Apply rotation using the sign of targetRotation for direction
                float actualRotation = currentRotation * Mathf.Sign(targetRotation);
                transform.rotation = startRotation * Quaternion.Euler(0, 0, actualRotation);
            }
        }
    }
}
