using UnityEngine;

public class SwitchableAppearing : Switchable
{
    [SerializeField] private GameObject _visual;
    private Collider _collider;

    private void Awake()
    {
        if (TryGetComponent(out Collider collider))
        {
            _collider = collider;
            collider.enabled = false;
        }

        if (_visual != null)
            _visual.SetActive(false);
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
