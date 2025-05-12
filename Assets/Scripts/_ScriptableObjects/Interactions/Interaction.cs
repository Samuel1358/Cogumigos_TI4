using UnityEngine;

public abstract class Interaction : ScriptableObject
{
    [SerializeField] protected bool _interactJustOnce;

    // Properties
    public bool InteractJustOnce { get { return _interactJustOnce; } }

    // Inherit Public Methods
    public abstract void Interact(Player player);
}
