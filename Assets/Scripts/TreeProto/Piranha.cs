using UnityEngine;

public class Piranha : MonoBehaviour
{
    [SerializeField] private float initialUpwardSpeed = 10f;
    [SerializeField] private float gravity = 9.8f;
    [SerializeField] private float pauseAtBottom = 0.5f;
    
    private float currentVelocity = 0f;
    private bool isMovingUp = true;
    private Vector3 bottomPosition;
    private bool isPaused = false;
    private float pauseTimer = 0f;
    
    private void Start()
    {
        bottomPosition = transform.position;
        currentVelocity = initialUpwardSpeed;
    }
    
    private void Update()
    {
        if (isPaused)
        {
            pauseTimer -= Time.deltaTime;
            if (pauseTimer <= 0f)
            {
                isPaused = false;
                isMovingUp = true;
                currentVelocity = initialUpwardSpeed;
            }
            return;
        }
        
        if (isMovingUp)
        {
            currentVelocity -= gravity * Time.deltaTime;
            if (currentVelocity <= 0f)
            {
                currentVelocity = 0f;
                isMovingUp = false;
            }
        }
        else
        {
            currentVelocity += gravity * Time.deltaTime;
        }
        
        float direction = isMovingUp ? 1f : -1f;
        transform.Translate(Vector3.up * (currentVelocity * direction * Time.deltaTime));
        
        if (!isMovingUp && transform.position.y <= bottomPosition.y)
        {
            transform.position = new Vector3(transform.position.x, bottomPosition.y, transform.position.z);
            isPaused = true;
            pauseTimer = pauseAtBottom;
            currentVelocity = 0f;
        }
    }
} 