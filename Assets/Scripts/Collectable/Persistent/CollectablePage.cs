using UnityEngine;

public class CollectablePage : CollectablePersistenceBase {
    [SerializeField] private GameObject _visual;

    protected override void SetCollectableInactive() {
        _visual.SetActive(false);
        WasCollected = true;
    }

    private void OnTriggerEnter(Collider collider) {
        if (!WasCollected) {
            AudioManager.Instance.PlaySFX("Collectable");
            SetCollectableInactive();
        }
    }
}
