using UnityEngine;

public class SwitchableAppearing : Switchable
{
    [SerializeField] private GameObject _visual;
    private Collider _collider;

    private void Awake()
    {
        _visual.SetActive(false);
    }

    public override void Activate() 
    {
        _visual.SetActive(true);

        NeedReset = true;
    }

    public override void Disable() { }

    // Resetable
    public override void ResetObject()
    {

    }
}
