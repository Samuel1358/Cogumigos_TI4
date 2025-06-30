using UnityEngine;
using UnityEngine.Events;

public abstract class NonPersistentCollectable : ResetableBase
{
    [SerializeField] private GameObject _visual;
    [SerializeField] private Collider _collider;
    [SerializeField] private UnityEvent _onCollect;
    private bool _wasCollected;

    private void Awake() {
        _wasCollected = false;
    }

    private void OnTriggerEnter(Collider collider) {
        if (!_wasCollected) {
            if (collider.transform.parent.TryGetComponent<Player>(out Player player)) {
                _visual.SetActive(false);
                _collider.enabled = false;
                _wasCollected = true;
                OnCollect(player.Inventory);
                _onCollect.Invoke();
            }
        }
    }

    override public void ResetObject() {
        if (_visual != null)
            _visual.SetActive(true);
        if (_collider != null)
            _collider.enabled = true;

        _wasCollected = false;
    }

    protected abstract void OnCollect(PlayerInventory invetoryToStore);
}
