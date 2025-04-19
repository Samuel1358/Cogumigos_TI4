using UnityEngine;

public abstract class PersistenceCollectableBaseSO : ScriptableObject {
    [field: SerializeField] public string ID { get; private set; }

    [ContextMenu("Generate id")]
    private void GenerateGuid() {
        ID = System.Guid.NewGuid().ToString();
    }

    public bool VerifyState() {
        return DataPersistenceManager.Instance.VerifyCollectableState(ID);
    }
}
