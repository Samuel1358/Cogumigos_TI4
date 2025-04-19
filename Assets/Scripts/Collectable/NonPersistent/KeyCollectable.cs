using UnityEngine;

public class KeyCollectable : NonPersistentCollectable {
    [SerializeField] private KeyTypes _keyType;
    protected override void OnCollect(PlayerInventory invetoryToStore) {
        invetoryToStore.CollectKey(_keyType);
    }
}
