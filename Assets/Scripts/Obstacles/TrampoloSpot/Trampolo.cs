using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Trampoline : MonoBehaviour
{
    public float bounceForce = 10f;             
    public float bounceCooldown = 0.2f;      

    private float lastBounceTime;               

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log(collision.gameObject);
        if (collision.collider.CompareTag("TriggerCheck"))
        {
            Debug.Log("2");
            Rigidbody playerRb = collision.transform.GetComponentInParent<Rigidbody>();
            Debug.Log("3");
            if (playerRb != null && Time.time >= lastBounceTime + bounceCooldown)
            {
                Debug.Log("4");
                Vector3 currentVelocity = playerRb.linearVelocity;
                playerRb.linearVelocity = new Vector3(currentVelocity.x, 0f, currentVelocity.z);

                Vector3 bounceVelocity = new Vector3(0, bounceForce, 0);
                playerRb.linearVelocity += bounceVelocity;

                lastBounceTime = Time.time;
            }
        }
    }
}
