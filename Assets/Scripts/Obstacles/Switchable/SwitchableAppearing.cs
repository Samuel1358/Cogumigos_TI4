using UnityEngine;

public class SwitchableAppearing : Switchable
{
    [SerializeField] private GameObject _visual;
    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _visual.SetActive(false);
        _collider.enabled = false;
    }

    public override void Activate() 
    {
        _visual.SetActive(true);
        _collider.enabled = true;

        NeedReset = true;
    }

    public override void Disable() { }

    // Resetable
    public override void ResetObject()
    {

    }
}
