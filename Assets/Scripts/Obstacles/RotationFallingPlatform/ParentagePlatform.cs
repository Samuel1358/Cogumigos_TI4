using UnityEngine;

public class ParentagePlatform : CollisionHandler
{
    // ParentageProperty

    private Transform _child = null;

    protected Transform child { get { return _child; } set { _child = SetChildProperty(value); } }

    private Transform SetChildProperty(Transform value)
    {
        if (value == null)
        {
            if (_child != null)
                _child.parent = null;
            return null;
        }

        if (_child != null)
            _child.parent = null;
        value.parent = transform;
        return value;
    }

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

    // Private Methods

    private void Enter(Transform transform)
    {
        while (transform.parent != null)
        {
            transform = transform.parent;
        }
        child = transform;
    }

    private void Exit(Transform transform)
    {
        child = null;
    }
}
