using UnityEngine;

public enum InteractiveObjectType
{
    None,
    Carry,
    Convey,
}

public abstract class InteractiveObject : MonoBehaviour
{
#pragma warning disable CS0649 // Campo "InteractiveObject.interactiveType" nunca é atribuído e sempre terá seu valor padrão 
    private InteractiveObjectType interactiveType;
#pragma warning restore CS0649 // Campo "InteractiveObject.interactiveType" nunca é atribuído e sempre terá seu valor padrão 

    // Get & Set
    public InteractiveObjectType GetInteractiveType()
    {
        return interactiveType;
    }

    // Public Methods
    public abstract void AssingFriendshroom();

    public abstract void PositionFriendshroom(Transform friendshroom);

    public abstract void RealeaseFriendshroom();
}
