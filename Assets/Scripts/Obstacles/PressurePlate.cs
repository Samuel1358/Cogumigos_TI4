using UnityEngine;

public class PressurePlate : Switch
{
    [SerializeField] private LayerMask includeLayers;
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
        Debug.Log("1");
        if ((includeLayers & (1 << other.gameObject.layer)) != 0)
        {
            Debug.Log("2");

            Activate(switchableObj);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("3");
        if ((includeLayers & (1 << other.gameObject.layer)) != 0)
        {
            Debug.Log("4");
            Disable(switchableObj);
        }
    }
}
