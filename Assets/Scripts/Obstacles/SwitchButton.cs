using UnityEngine;

public class SwitchButton : Switch
{
    [SerializeField] LayerMask includeLayers;
    [SerializeField] ISwitchable switchableObj;

    // Private Methods
    protected override void Activate(ISwitchable obj)
    {
        obj.Activate();
    }

    protected override void Disable(ISwitchable obj) { }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == includeLayers)
        {
            Activate(switchableObj);
        }
    }

}
