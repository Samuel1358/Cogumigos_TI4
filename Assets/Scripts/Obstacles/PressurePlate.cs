using UnityEngine;

public class PressurePlate : Switch
{
    [SerializeField] private LayerMask includeLayers;
    [SerializeReference] private GameObject switchableObj;
    /*private ISwitchable _switchableObj;

    private void Start()
    {
        if 
    }*/

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
        //Debug.Log(other.gameObject.layer);
        //Debug.Log(includeLayers.value);
        if (other.CompareTag("Player") || other.CompareTag("TriggerCheck") || other.CompareTag("Friendshroom"))
        {
            Debug.Log("2");
            
            if (switchableObj.TryGetComponent(out ISwitchable obj))
                Activate(obj);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("3");
        if (other.CompareTag("Player") || other.CompareTag("TriggerCheck") || other.CompareTag("Friendshroom"))
        {
            Debug.Log("4");
            if (switchableObj.TryGetComponent(out ISwitchable obj))
                Disable(obj);
        }
    }
}
