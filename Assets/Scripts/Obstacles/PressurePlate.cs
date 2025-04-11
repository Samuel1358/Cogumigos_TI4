using UnityEngine;

public class PressurePlate : Switch
{
    [SerializeField] private LayerMask includeLayers;
    [SerializeReference] private GameObject switchableObj;

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
        if (other.CompareTag("Player") || other.CompareTag("TriggerCheck") || other.CompareTag("Friendshroom"))
        {
            Debug.Log("2");
            
            if (switchableObj.TryGetComponent(out Switchable obj))
                Activate(obj);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("3");
        if (other.CompareTag("Player") || other.CompareTag("TriggerCheck") || other.CompareTag("Friendshroom"))
        {
            Debug.Log("4");
            if (switchableObj.TryGetComponent(out Switchable obj))
                Disable(obj);
        }
    }
}
