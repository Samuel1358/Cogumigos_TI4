using DG.Tweening;
using UnityEngine;

public class SychroningRotatePlatform : CollisionHandler
{
    // ParentageProperty

    private Transform _child = null;
    private Quaternion _lastRot;

    protected Transform child { get { return _child; } set { _child = value; } }

    #region // Inherited Public Methods

    protected override void CollisionEnter(Collision collision)
    {
        Enter(collision.collider.transform);
    }

    protected override void CollisionExit(Collision collision)
    {
        Exit(collision.collider.transform);
    }

    protected override void TriggerEnter(Collider other)
    {
        Enter(other.transform);
    }

    protected override void TriggerExit(Collider other)
    {
        Exit(other.transform);
    }

    protected override void ChildCollisionEnter(Collision collision)
    {
        Enter(collision.collider.transform);
    }

    protected override void ChildCollisionExit(Collision collision)
    {
        Exit(collision.collider.transform);
    }

    protected override void ChildTriggerEnter(Collider other)
    {
        Enter(other.transform);
    }

    protected override void ChildTriggerExit(Collider other)
    {
        Exit(other.transform);
    }

    #endregion

    private void FixedUpdate()
    {
        Sync();
    }

    // Private Methods
    private void Enter(Transform transform)
    {
        while (transform.parent != null)
        {
            transform = transform.parent;
        }
        child = transform;
    }

    private void Sync()
    {        
        if (child != null)
        {
            Quaternion currentRot = transform.rotation;
            Quaternion deltaRot = currentRot * Quaternion.Inverse(_lastRot);

            if (child.TryGetComponent(out Rigidbody rb))
            {
                rb.MovePosition(deltaRot * (child.position - transform.position) + transform.position);
                rb.MoveRotation(deltaRot * child.rotation);
            }
            else
            {
                child.position = deltaRot * (child.position - transform.position) + transform.position;
                child.rotation = deltaRot * child.rotation;
            }                    
        }
        _lastRot = transform.rotation;
    }

    private void Exit(Transform transform)
    {
        child = null;
    }
}
