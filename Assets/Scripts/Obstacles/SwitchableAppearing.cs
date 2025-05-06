using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SwitchableAppearing : Switchable
{
    [SerializeField] private GameObject _visual;
    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _visual.SetActive(false);
    }

    public override void Activate() 
    {
        _visual.SetActive(true);
        _collider.enabled = true;
    }

    public override void Disable() { }
}
