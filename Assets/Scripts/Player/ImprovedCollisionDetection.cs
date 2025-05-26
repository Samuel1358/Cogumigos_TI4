using UnityEngine;

public class ImprovedCollisionDetection : MonoBehaviour {
    public bool IsColliding { get; set; } = false;
    public Vector3 CollisionDirection { get; private set; }

    private void OnCollisionEnter(Collision collision) {
        IsColliding = true;
        foreach (ContactPoint contact in collision.contacts) {
            Debug.Log("Ponto de contato: " + contact.point);
        }
    }
    private void OnCollisionExit(Collision collision) {
        IsColliding = false;
    }
}
