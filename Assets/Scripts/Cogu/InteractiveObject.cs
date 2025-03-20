using UnityEngine;

public enum InteractiveObjectType
{
    None,
    Carry,
    Convey,
}

public abstract class InteractiveObject : MonoBehaviour
{
    private InteractiveObjectType interactiveType;

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
