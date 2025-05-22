using UnityEngine;

public class RollingTrunk : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float pushForce = 5f;
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
            playerRb.AddForce((pushToRight ? transform.right : -transform.right) * pushForce, ForceMode.Force);
        }
    }
}
