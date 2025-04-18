using UnityEngine;

public abstract class Switch : MonoBehaviour
{
    // Inherit Methods
    protected abstract void Activate(Switchable obj);

    protected abstract void Disable(Switchable obj);
}
