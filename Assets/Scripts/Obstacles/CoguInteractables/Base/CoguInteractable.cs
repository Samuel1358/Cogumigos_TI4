using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class CoguInteractable : ResetableBase
{
    // Fields
    [SerializeField] private CoguType _assignedCoguType;
    [SerializeField] private float _interactDistance;
    
    protected bool _isAvailable = true;

    // Properties
    public CoguType AssignedCoguType { get { return _assignedCoguType; } }
    public float InteractDistance {  get { return _interactDistance; } }
    public bool IsAvailable {  get { return _isAvailable; } }

    // Public Methods
    public virtual void EnableInteract()
    {
        _isAvailable = true;
    }

    public virtual void DisableInteract()
    {
        _isAvailable = false;
    }

    // Inherit Public Methods
    public abstract Action Interact(Cogu cogu);

    protected virtual void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.up, _interactDistance);
        Handles.color = Color.white;
#endif
    }
}