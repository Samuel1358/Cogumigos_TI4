using UnityEngine;

public abstract class CollectablePersistenceBase : MonoBehaviour, IDataPersistence {

    protected bool _wasCollected;

    public bool WasCollected { get { return _wasCollected; } }

    [field: SerializeField] public PersistenteCollectableDataSO CollectableSO { get; private set; }

    private string _id;

    private void Awake() {
        _id = CollectableSO.ID;
    }

    public void LoadData(GameData data) {
        data.Colectables.TryGetValue(_id, out _wasCollected);
        if (_wasCollected) {
            SetCollectableInactive();
        }
    }

    public void SaveData(ref GameData data) {
        if (data.Colectables.ContainsKey(_id)) {
            data.Colectables.Remove(_id);
        }
        data.Colectables.Add(_id, _wasCollected);
    }

    private void OnValidate() {
        _id = CollectableSO?.ID;
    }

    protected abstract void SetCollectableInactive();
}
