using System;
using UnityEngine;
using UnityEditor;

public class CollisionHandler : MonoBehaviour
{
    protected enum CollideMode
    {
        None,
        ParentCollision,
        ParentTrigger,
        ParentBoth,
        ChildCollision,
        ChildTrigger,
        ChildBoth,
        ParentCollisionChildTrigger,
        ParentTriggerChildCollision,
        BothColision,
        BothTrigger,
        All
    }

    [SerializeField, HideInInspector] protected CollideMode _collideMode;
    [SerializeField, HideInInspector] protected LayerMask _includeLayers;

    public virtual bool lockColliderMode { get {  return false; } }

    #region // InheritExecuteMethods

    protected virtual void CollisionEnter(Collision collision) { }
    protected virtual void CollisionExit(Collision collision) { }
    protected virtual void TriggerEnter(Collider other) { }
    protected virtual void TriggerExit(Collider other) { }

    protected virtual void ChildCollisionEnter(Collision collision) { }
    protected virtual void ChildCollisionExit(Collision collision) { }
    protected virtual void ChildTriggerEnter(Collider other) { }
    protected virtual void ChildTriggerExit(Collider other) { }

    #endregion

    #region // CollisionTrigger

    private void OnCollision(Collision collision, Action<Collision> action)
    {
        if (_collideMode == CollideMode.ParentCollision || _collideMode == CollideMode.ParentBoth || _collideMode == CollideMode.ParentCollisionChildTrigger || _collideMode == CollideMode.BothColision || _collideMode == CollideMode.All)
        {
            //CollisionDebug("Collision Enter", collision.gameObject.name, collision.collider.name);
            if ((_includeLayers & (1 << collision.collider.gameObject.layer)) != 0)
            {
                ValideLayerDebug();
                action.Invoke(collision);
            }
        }
    }

    private void OnTrigger(Collider other, Action<Collider> action)
    {
        if (_collideMode == CollideMode.ParentTrigger || _collideMode == CollideMode.ParentBoth || _collideMode == CollideMode.ParentTriggerChildCollision || _collideMode == CollideMode.BothTrigger || _collideMode == CollideMode.All)
        {
            //CollisionDebug("Trigger Enter", other.gameObject.name, other.name);
            if ((_includeLayers & (1 << other.gameObject.layer)) != 0)
            {
                ValideLayerDebug();
                action.Invoke(other);
            }
        }
    }

    private void OnChildCollision(Collision collision, Action<Collision> action, Action<Collision> childAction)
    {
        if (_collideMode == CollideMode.ChildCollision || _collideMode == CollideMode.ChildBoth || _collideMode == CollideMode.ParentTriggerChildCollision || _collideMode == CollideMode.BothColision || _collideMode == CollideMode.All)
        {
            //CollisionDebug("Child Collision Enter", collision.gameObject.name, collision.collider.name);
            if ((_includeLayers & (1 << collision.collider.gameObject.layer)) != 0)
            {
                ValideLayerDebug();
                action.Invoke(collision);
                childAction.Invoke(collision);
            }
        }             
    }

    private void OnChildTrigger(Collider other, Action<Collider> action, Action<Collider> childAction)
    {
        if (_collideMode == CollideMode.ChildTrigger || _collideMode == CollideMode.ChildBoth || _collideMode == CollideMode.ParentCollisionChildTrigger || _collideMode == CollideMode.BothTrigger || _collideMode == CollideMode.All)
        {
            //CollisionDebug("Child Trigger Enter", other.gameObject.name, other.name);
            if ((_includeLayers & (1 << other.gameObject.layer)) != 0)
            {
                ValideLayerDebug();
                action.Invoke(other);
                childAction.Invoke(other);
            }
        }       
    }

    #endregion

    #region // OnParent

    private void OnCollisionEnter(Collision collision)
    {
        //CollisionDebug("Collision Enter", collision.gameObject.name, collision.collider.name);
        OnCollision(collision, CollisionEnter);
    }

    private void OnCollisionExit(Collision collision)
    {
        //CollisionDebug("Collision Exit", collision.gameObject.name, collision.collider.name);
        OnCollision(collision, CollisionExit);
    }

    private void OnTriggerEnter(Collider other)
    {
        //CollisionDebug("Trigger Enter", other.gameObject.name, other.name);
        OnTrigger(other, TriggerEnter);
    }

    private void OnTriggerExit(Collider other)
    {
        //CollisionDebug("Trigger Exit", other.gameObject.name, other.name);
        OnTrigger(other, TriggerExit);
    }

    #endregion

    #region // OnChild

    public void OnChildCollisionEnter(Collision collision, CollisionHandlerChild child)
    {
        OnChildDebug(child);
        OnChildCollision(collision, ChildCollisionEnter, child.ParentCollisionEnter);
    }

    public void OnChildCollisionExit(Collision collision, CollisionHandlerChild child)
    {
        OnChildDebug(child);
        OnChildCollision(collision, ChildCollisionExit, child.ParentCollisionExit);
    }

    public void OnChildTriggerEnter(Collider other, CollisionHandlerChild child)
    {
        OnChildDebug(child);
        OnChildTrigger(other, ChildTriggerEnter, child.ParentTriggerEnter);
    }

    public void OnChildTriggerExit(Collider other, CollisionHandlerChild child)
    {
        OnChildDebug(child);
        OnChildTrigger(other, ChildTriggerExit, child.ParentTriggerExit);
    }

    #endregion


    // DELETAR
    private void CollisionDebug(string collisionType, string objName, string colliderName)
    {
        //Debug.Log($"{collisionType}: {objName} | Collider: {colliderName}");
    }

    private void ValideLayerDebug()
    {
        //Debug.Log("Valide layer");
    }

    private void OnChildDebug(CollisionHandlerChild child)
    {
        //Debug.Log($"Child: {child.name}");
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(CollisionHandler), true)]
public class CollisionHandlerInspector : Editor
{
    private CollisionHandler _collisionHandler;
    private SerializedObject _serializedObject;

    private SerializedProperty _collideMode;
    private SerializedProperty _includeLayers;

    private void OnEnable()
    {
        _collisionHandler = target as CollisionHandler;
        _serializedObject = new SerializedObject(_collisionHandler);

        _collideMode = _serializedObject.FindProperty("_collideMode");
        _includeLayers = _serializedObject.FindProperty("_includeLayers");
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SerializedFields();
    }

    #region // OnInspectorGUI

    private void SerializedFields()
    {
        _serializedObject.Update();

        Header("Collision Handler");
        if (!_collisionHandler.lockColliderMode)
            EditorGUILayout.PropertyField(_collideMode);
        EditorGUILayout.PropertyField(_includeLayers);

        _serializedObject.ApplyModifiedProperties();
    }

    void Header(string title)
    {
        EditorGUILayout.Space(8);
        EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
    }

    #endregion
}
#endif
