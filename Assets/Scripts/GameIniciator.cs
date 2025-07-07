using DG.Tweening;
using DialogSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameIniciator : MonoBehaviour {
    public static GameIniciator Instance { get; private set; }
    [field: SerializeField] public RespawnController RespawnControllerInstance { get; private set; }
    [field: SerializeField] public CoguManager CoguManagerInstance { get; private set; }
    [field: SerializeField] public AudioManager AudioManagerInstance { get; private set; }
    [field: SerializeField] public Cheats CheatsInstance { get; private set; }
    [field: SerializeField] public GameObject FrameDebuggerPrefab { get; private set; }
    [field: SerializeField] public GameManager GameManagerInstance { get; private set; }
    [field: SerializeField] public DataPersistenceManager PersistenceManagerInstance { get; private set; }
    [field: SerializeField] public DialogController DialogManagerInstance { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            Inicialize();
            SetUp();
            DontDestroyOnLoad(this);
        }
        else {
            Destroy(this);
        }
    }

    public void LoadScenes(int sceneIndex) {
        DOTween.KillAll();
        if (sceneIndex == 0) LoadMenuScene();
        else {           
            LoadGameScene(sceneIndex);
        }
    }
    private void LoadMenuScene() {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
    private void LoadGameScene(int sceneIndex) {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
    }

    public void PauseGame() {
        Time.timeScale = 0f;
        InGameMenuInitiator.Instance.PauseGameUI();
    }
    
    public void UnpauseGame() {
        Time.timeScale = 1f;
        InGameMenuInitiator.Instance.UnPauseGameUI();
    }
    public void GameOver() {
        Time.timeScale = 0f;
        InGameMenuInitiator.Instance.GameOverUI();
    }

    private void Inicialize() {
        RespawnControllerInstance = Instantiate(RespawnControllerInstance, transform);
        CoguManagerInstance = Instantiate(CoguManagerInstance, transform);
        AudioManagerInstance = Instantiate(AudioManagerInstance, transform);
        CheatsInstance = Instantiate(CheatsInstance, transform);
        FrameDebuggerPrefab = Instantiate(FrameDebuggerPrefab, transform);
        GameManagerInstance = Instantiate(GameManagerInstance, transform);
        PersistenceManagerInstance = Instantiate(PersistenceManagerInstance, transform);
        DialogManagerInstance = Instantiate(DialogManagerInstance, transform);
    }
    private void SetUp() {
        GameManagerInstance.WakeUp(FrameDebuggerPrefab);
    }
}
