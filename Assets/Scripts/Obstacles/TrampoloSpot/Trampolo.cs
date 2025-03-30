using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Trampoline : MonoBehaviour
{
    public float bounceForce = 10f;             
    public float bounceCooldown = 0.2f;      

    private float lastBounceTime;               

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("TriggerCheck"))
        {
            Rigidbody playerRb = collision.transform.GetComponentInParent<Rigidbody>();

            if (playerRb != null && Time.time >= lastBounceTime + bounceCooldown)
            {
                Vector3 currentVelocity = playerRb.linearVelocity;
                playerRb.linearVelocity = new Vector3(currentVelocity.x, 0f, currentVelocity.z);

                Vector3 bounceVelocity = new Vector3(0, bounceForce, 0);
                playerRb.linearVelocity += bounceVelocity;

                lastBounceTime = Time.time;
            }
        }
    }
}
