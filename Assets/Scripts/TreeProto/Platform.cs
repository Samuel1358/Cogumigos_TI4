using UnityEngine;

public class Platform : MonoBehaviour
{
    private float speed;
    private bool moveRight;
    private Vector3 startPosition;
    private Vector3 resetPosition;
    private float resetXPosition;

    public void Initialize(float platformSpeed, bool rightDirection, Vector3 resetPos, float resetX)
    {
        speed = platformSpeed;
        moveRight = rightDirection;
        startPosition = transform.position;
        resetPosition = resetPos;
        resetXPosition = resetX;
    }

    void Update()
    {
        // Calculate movement
        float movement = speed * Time.deltaTime;
        if (!moveRight)
        {
            movement = -movement;
        }

        // Update position
        transform.Translate(new Vector3(movement, 0, 0));

        // Check if platform needs to reset based on X position
        if (moveRight && transform.position.x >= resetXPosition)
        {
            transform.position = resetPosition;
        }
        else if (!moveRight && transform.position.x <= resetXPosition)
        {
            transform.position = resetPosition;
        }
    }
} 