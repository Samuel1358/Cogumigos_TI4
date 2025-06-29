using UnityEngine;

public abstract class CollectablePersistenceBase : MonoBehaviour, IDataPersistence {

    protected bool WasCollected;

    [SerializeField] protected PersistenteCollectableDataSO _collectableSO;

    private string _id;

    private void Awake() {
        _id = _collectableSO.ID;
    }

    public void LoadData(GameData data) {
        data.Colectables.TryGetValue(_id, out WasCollected);
        if (WasCollected) {
            SetCollectableInactive();
        }
    }

    public void SaveData(ref GameData data) {
        if (data.Colectables.ContainsKey(_id)) {
            data.Colectables.Remove(_id);
        }
        data.Colectables.Add(_id, WasCollected);
    }

    private void OnValidate() {
        _id = _collectableSO?.ID;
    }

    protected abstract void SetCollectableInactive();
}
