using UnityEngine;

public class SwitchButton : Switch
{
    [SerializeField] LayerMask includeLayers;
    [SerializeField] Switchable switchableObj;

    // Private Methods
    protected override void Activate(Switchable obj)
    {
        obj.Activate();
    }

    protected override void Disable(Switchable obj) { }


    private void OnTriggerEnter(Collider other)
    {
        if ((includeLayers & (1 << other.gameObject.layer)) != 0)
        {
            Activate(switchableObj);
        }
    }

}
