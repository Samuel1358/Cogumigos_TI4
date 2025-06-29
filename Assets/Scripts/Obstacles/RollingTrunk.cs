using UnityEngine;

public class RollingTrunk : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private Transform visualChild;
    [SerializeField] private bool pushToRight = false;
    private Vector3 rotationDirection;

    private void Start()
    {
        if (visualChild == null && transform.childCount > 0)
        {
            visualChild = transform.GetChild(0);
        }

        if (visualChild == null)
        {
            Debug.LogWarning("No visual child assigned to RollingTrunk. Please assign a child object to rotate.");
        }

        rotationDirection = new Vector3(pushToRight ? 1f : -1f, 0f, 0f);
    }

    private void Update()
    {
        if (visualChild != null)
        {
            visualChild.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionStay(Collision collision)
    {

        Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            //Vector3 raio = collision.transform.position - transform.position;
            Vector3 dir = Vector3.Cross(transform.forward, collision.contacts[0].normal);
            playerRb.transform.position += -dir * Time.deltaTime;
            //playerRb.AddForce(-dir * 0.2f, ForceMode.VelocityChange);
            Debug.DrawLine(collision.transform.position, collision.transform.position - dir * 5);
            //playerRb.AddForce((pushToRight ? transform.right : -transform.right) * pushForce, ForceMode.Force);
        }
    }
}