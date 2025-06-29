using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class DataPersistenceManager : MonoBehaviour
{
    public static DataPersistenceManager Instance { get; private set; }

    [SerializeField] private string _fileName;

    private GameData _gameData;
    private List<IDataPersistence> _dataPersistencesObjects;
    private FileDataHandler _dataHandler;

    private void Awake() {
        if (Instance == null) {
            Instance = this; 
        }
        else {
            Debug.LogError("Mais de uma Instancia do DataPersistenceManager na cena");
            Destroy(gameObject);
        }
    }

    private void Start() {
        _dataHandler = new FileDataHandler(Application.persistentDataPath, _fileName);
        _dataPersistencesObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame() {
        this._gameData = new GameData();
    }

    public void LoadGame() {
        _gameData = _dataHandler.Load();

        if (this._gameData == null) {
            Debug.Log("Sem dados de jogo criado. Inicializando com valores padrões");
            NewGame();
        }
        foreach(IDataPersistence dataPersistencesObject in _dataPersistencesObjects) {
            dataPersistencesObject.LoadData(_gameData);
        }
    }

    public void SaveGame() {
        foreach (IDataPersistence dataPersistencesObject in _dataPersistencesObjects) {
            dataPersistencesObject.SaveData(ref _gameData);
        }

        _dataHandler.Save(_gameData);
    }

    private void OnApplicationQuit() {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects() {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public bool VerifyCollectableState(string idToCheck) {
        return _gameData.Colectables.ContainsKey(idToCheck);
    }
    
    public bool VerifyCollectableCollected(string idToCheck) {
        return _gameData.Colectables[idToCheck];
    }
}
