using UnityEngine;

public class PressurePlate : Switch
{
    [SerializeField] LayerMask includeLayers;
    [SerializeField] ISwitchable switchableObj;

    // Private Methods
    protected override void Activate(ISwitchable obj)
    {
        obj.Activate();
    }

    protected override void Disable(ISwitchable obj)
    {
        obj.Disable();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == includeLayers)
        {
            Activate(switchableObj);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == includeLayers)
        {
            Disable(switchableObj);
        }
    }
}
