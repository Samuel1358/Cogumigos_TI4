using System;
using UnityEngine;

public abstract class CoguInteractable : ResetableBase
{
    // Fields
    [SerializeField] CoguType _assignedCoguType;
    protected bool _isAvailable = true;

    // Properties
    public CoguType AssignedCoguType { get { return _assignedCoguType; } }
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
}