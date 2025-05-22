using UnityEngine;

public class PressurePlate : Switch
{
    [SerializeReference] private Switchable switchableObj;

    // Private Methods
    protected override void Activate(Switchable obj)
    {
        obj.Activate();
    }

    protected override void Disable(Switchable obj)
    {
        obj.Disable();
    }


    private void OnTriggerEnter(Collider other)
    {
        Activate(switchableObj);
    }

    private void OnTriggerExit(Collider other)
    {
        Disable(switchableObj);
    }
}
