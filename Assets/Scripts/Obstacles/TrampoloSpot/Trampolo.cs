using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Trampoline : MonoBehaviour
{
    public float bounceForce = 100f;             
    public float bounceCooldown = 0.2f;      

    private float lastBounceTime;               

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log(collision.gameObject);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("2");
            Rigidbody playerRb = collision.transform.GetComponentInParent<Rigidbody>();
            Debug.Log("3");
            if (playerRb != null && Time.time >= lastBounceTime + bounceCooldown)
            {
                Debug.Log("4");
                Vector3 bounceVelocity = new Vector3(0, bounceForce, 0);
                playerRb.AddForce(bounceVelocity, ForceMode.VelocityChange);

                lastBounceTime = Time.time;
            }
        }
    }
}
