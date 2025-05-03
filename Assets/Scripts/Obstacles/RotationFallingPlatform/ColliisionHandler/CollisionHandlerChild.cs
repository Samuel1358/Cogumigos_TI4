using UnityEngine;

public class CollisionHandlerChild : MonoBehaviour
{
    private CollisionHandler _parent;

    protected virtual void Start()
    {
        _parent = GetComponentInParent<CollisionHandler>();
    }

    #region // InheritExecuteMethods

    public virtual void ParentCollisionEnter(Collision collision) { }

    public virtual void ParentCollisionExit(Collision collision) { }

    public virtual void ParentTriggerEnter(Collider other) { }

    public virtual void ParentTriggerExit(Collider other) { }

    #endregion

    #region // CallMethods

    protected void CallCollisionEnter(Collision collision) 
    {
        if (_parent != null) 
        {
            _parent.OnChildCollisionEnter(collision, this);
        }
    }

    protected void CallCollisionExit(Collision collision) 
    {
        if (_parent != null) 
        {
            _parent.OnChildCollisionExit(collision, this);
        }
    }

    protected void CallTriggerEnter(Collider other) 
    {
        if (_parent != null) 
        {
            _parent.OnChildTriggerEnter(other, this);
        }
    }

    protected void CallTriggerExit(Collider other) 
    {
        if (_parent != null) 
        {
            _parent.OnChildTriggerExit(other, this);
        }
    }

    #endregion

    protected virtual void OnCollisionEnter(Collision collision)
    {
        CallCollisionEnter(collision);
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
        CallCollisionExit(collision);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        CallTriggerEnter(other);
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        CallTriggerExit(other);
    }
}
