using UnityEngine;

public class SwitchableAppearing : Switchable {
    [SerializeField] private GameObject _visual;

    private void Awake() {
        if (TryGetComponent(out Collider collider)) {
            collider.enabled = false;
        }

        if (_visual != null)
            _visual.SetActive(false);
    }

    public override void Activate() {
        _visual.SetActive(true);
        NeedReset = true;
    }

    public override void Disable() { }

    // Resetable
    public override void ResetObject() {

    }
}
