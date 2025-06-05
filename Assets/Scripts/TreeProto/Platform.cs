using UnityEngine;

public class Platform : MonoBehaviour
{
    private float speed;
    private bool moveRight;
    private Vector3 resetPosition;
    private float resetXPosition;
    private Transform playerOnPlatform;

    public void Initialize(float platformSpeed, bool rightDirection, Vector3 resetPos, float resetX)
    {
        speed = platformSpeed;
        moveRight = rightDirection;
        resetPosition = resetPos;
        resetXPosition = resetX;
    }

    void Update()
    {
        Vector3 currentPosition = transform.position;
        
        float movement = speed * Time.deltaTime;
        if (!moveRight) movement = -movement;

        transform.Translate(new Vector3(movement, 0, 0));

        if (playerOnPlatform != null)
        {
            Vector3 platformMovement = transform.position - currentPosition;
            playerOnPlatform.Translate(platformMovement, Space.World);
        }

        if ((moveRight && transform.position.x >= resetXPosition) || 
            (!moveRight && transform.position.x <= resetXPosition))
        {
            Vector3 resetMovement = resetPosition - transform.position;
            transform.position = resetPosition;
            
            if (playerOnPlatform != null)
            {
                playerOnPlatform.Translate(resetMovement, Space.World);
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        Transform player = GetPlayerTransform(collision);
        if (player != null)
        {
            playerOnPlatform = player;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (GetPlayerTransform(collision) != null)
        {
            playerOnPlatform = null;
        }
    }

    private Transform GetPlayerTransform(Collision collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player == null)
        {
            player = collision.gameObject.GetComponentInParent<Player>();
        }
        return player?.transform;
    }
} 