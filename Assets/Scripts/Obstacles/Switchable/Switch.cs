using UnityEngine;

public abstract class Switch : MonoBehaviour
{
    // Inherit Methods
    protected abstract void Activate(ISwitchable obj);

    protected abstract void Disable(ISwitchable obj);
}
