using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    protected enum CollideMode
    {
        Collision,
        Trigger,
        Both
    }
    [SerializeField] protected CollideMode _collideMode;
    [SerializeField] protected LayerMask _includeLayers;

    protected virtual void CollisionEnter(Collision collision) { }
    protected virtual void CollisionExit(Collision collision) { }
    protected virtual void TriggerEnter(Collider other) { }
    protected virtual void TriggerExit(Collider other) { }

    private void OnCollisionEnter(Collision collision)
    {
        if (_collideMode == CollideMode.Collision || _collideMode == CollideMode.Both)
            Debug.Log($"Collision Enter: {collision.gameObject.name}");
            if ((_includeLayers & (1 << collision.gameObject.layer)) != 0)
            {
                collision.transform.parent = transform;
                CollisionEnter(collision);
            }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (_collideMode == CollideMode.Collision || _collideMode == CollideMode.Both)
            Debug.Log($"Collision Exit: {collision.gameObject.name}");
            if ((_includeLayers & (1 << collision.gameObject.layer)) != 0)
            {
                collision.transform.parent = null;
                CollisionExit(collision);
            }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_collideMode == CollideMode.Trigger || _collideMode == CollideMode.Both)
            Debug.Log($"Trigger Enter: {other.gameObject.name}");
            if ((_includeLayers & (1 << other.gameObject.layer)) != 0)
            {
                other.transform.parent = transform;
                TriggerEnter(other);
            }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_collideMode == CollideMode.Trigger || _collideMode == CollideMode.Both)
            Debug.Log($"Trigger Exit: {other.gameObject.name}");
            if ((_includeLayers & (1 << other.gameObject.layer)) != 0)
            {
                other.transform.parent = null;
                TriggerExit(other);
            }
    }
}
