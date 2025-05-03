using System;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    protected enum CollideMode
    {
        None,
        Collision,
        Trigger,
        Both,
        ChildCollision,
        ChildTrigger,
        ChildBoth,
    }
    [SerializeField] protected CollideMode _collideMode;
    [SerializeField] protected LayerMask _includeLayers;

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

        if (value == _child) 
        {
            value.parent = null;
            return null;
        } 
        else 
        { 
            if (_child != null) 
                _child.parent = null; 
            value.parent = transform;
            return value; 
        }
    }


    protected virtual void CollisionEnter(Collision collision) { }
    protected virtual void CollisionExit(Collision collision) { }
    protected virtual void TriggerEnter(Collider other) { }
    protected virtual void TriggerExit(Collider other) { }

    protected virtual void ChildCollisionEnter(Collision collision) { }
    protected virtual void ChildCollisionExit(Collision collision) { }
    protected virtual void ChildTriggerEnter(Collider other) { }
    protected virtual void ChildTriggerExit(Collider other) { }

    private void OnCollision(Collision collision, Action<Transform> parentage, Action<Collision> action)
    {
        Debug.Log($"Collision Enter: {collision.gameObject.name} | Collider: {collision.collider.name}");
        if (_collideMode == CollideMode.Collision || _collideMode == CollideMode.Both)
        {
            //Debug.Log("Valide Mode");
            if ((_includeLayers & (1 << collision.collider.gameObject.layer)) != 0)
            {
                parentage.Invoke(collision.collider.transform);

                action.Invoke(collision);
            }
        }
    }

    private void OnTrigger(Collider other, Action<Transform> parentage, Action<Collider> action)
    {
        Debug.Log($"Trigger Enter: {other.gameObject.name} | Collider: {other.name}");
        if (_collideMode == CollideMode.Trigger || _collideMode == CollideMode.Both)
        {
            //Debug.Log("Valide Mode");
            if ((_includeLayers & (1 << other.gameObject.layer)) != 0)
            {
                parentage.Invoke(other.transform);

                action.Invoke(other);
            }
        }
    }

    private void OnChildCollision(Collision collision, Action<Transform> parentage, Action<Collision> action)
    {
        Debug.Log($"Collision Enter: {collision.gameObject.name} | Collider: {collision.collider.name}");
        if (collision.collider.TryGetComponent(out MovingPlatformChild child))
        {
            if (_collideMode == CollideMode.ChildCollision || _collideMode == CollideMode.ChildBoth)
            {
                //Debug.Log("Valide Mode");
                if ((_includeLayers & (1 << collision.collider.gameObject.layer)) != 0)
                {
                    parentage.Invoke(collision.collider.transform);

                    action.Invoke(collision);
                }
            }
        }       
    }

    private void OnChildTrigger(Collider other, Action<Transform> parentage, Action<Collider> action)
    {
        Debug.Log($"Trigger Enter: {other.gameObject.name} | Collider: {other.name}");
        if (other.TryGetComponent(out MovingPlatformChild child))
        {
            if (_collideMode == CollideMode.ChildTrigger || _collideMode == CollideMode.ChildBoth)
            {
                //Debug.Log("Valide Mode");
                if ((_includeLayers & (1 << other.gameObject.layer)) != 0)
                {
                    parentage.Invoke(other.transform);

                    action.Invoke(other);
                }
            }
        }
    }

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
        if (child != null)
        {
            while (transform.parent != null)
            {
                transform = transform.parent;
            }

            if (child == transform)
                child = transform;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        OnCollision(collision, Enter, CollisionEnter);
    }

    private void OnCollisionExit(Collision collision)
    {
        OnCollision(collision, Exit, CollisionExit);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTrigger(other, Enter, TriggerEnter);
    }

    private void OnTriggerExit(Collider other)
    {
        OnTrigger (other, Exit, TriggerExit);
    }

    public void OnChildCollisionEnter(Collision collision)
    {
        OnChildCollision(collision, Enter, ChildCollisionEnter);
    }

    public void OnChildCollisionExit(Collision collision)
    {
        OnChildCollision(collision, Exit, ChildCollisionExit);
    }

    public void OnChildTriggerEnter(Collider other)
    {
        OnChildTrigger(other, Enter, ChildTriggerEnter);
    }

    public void OnChildTriggerExit(Collider other)
    {
        OnChildTrigger(other, Exit, ChildTriggerExit);
    }
}
