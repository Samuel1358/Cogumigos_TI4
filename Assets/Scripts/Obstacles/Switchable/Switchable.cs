using UnityEngine;

public abstract class Switchable : ResetableBase
{
    // Public Inherit Methods
    public abstract void Activate();

    public abstract void Disable();
}
