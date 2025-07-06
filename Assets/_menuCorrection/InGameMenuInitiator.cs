using UnityEngine;
using TMPro;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class InGameMenuInitiator : MonoBehaviour {
    [field: SerializeField] public static InGameMenuInitiator Instance { get; private set; }
    [field: SerializeField] public CheatsUI CheatCanvas { get; private set; }
    [field: SerializeField] public UiInventory InventoryCanvas { get; private set; }
    [field: SerializeField] public GameObject PauseCanvas { get; private set; }
    [field: SerializeField] public GameObject GameOverCanvas { get; private set; }
    [field: SerializeField] public bool IsGamePaused { get; private set; } = false;

    [SerializeField] private GameObject _dialogPanel;
    [SerializeField] private Image _speakerPortrait;
    [SerializeField] private TextMeshProUGUI _speakerNameText;
    [SerializeField] private TextMeshProUGUI _messageText;

    private void Awake() {
        Instance = this;
        GameIniciator.Instance.DialogManagerInstance.dialogUI.SetBarks(_dialogPanel, _speakerPortrait, _speakerNameText, _messageText);
    }

    public void PauseGameUI() {
        PauseCanvas.SetActive(true);
        GameIniciator.Instance.GameManagerInstance.UnhideMouse();
        IsGamePaused = true;
    }

    public void UnPauseGame() {
        GameIniciator.Instance.UnpauseGame();
    }
    public void UnPauseGameUI() {
        PauseCanvas.SetActive(false);
        GameIniciator.Instance.GameManagerInstance.HideMouse();
        IsGamePaused = false;
    }

    public void GameOverUI() {
        GameOverCanvas.SetActive(true);
        GameIniciator.Instance.GameManagerInstance.UnhideMouse();
    }

    public void GameOver() {
        GameIniciator.Instance.GameOver();
    }

    private void Start() {
        GameIniciator.Instance.RespawnControllerInstance.OnPlayerRespawn += OnPlayerDied;
        GameIniciator.Instance.PersistenceManagerInstance.UpdateAndLoad();
        InventoryCanvas.UpdateCoguCountUI(GameIniciator.Instance.GameManagerInstance.Player.CoguCast.CoguCount);
    }

    private void OnPlayerDied() {
        InventoryCanvas.UpdateCoguCountUI(GameIniciator.Instance.GameManagerInstance.Player.CoguCast.CoguCount);
    }

    public void LoadScene(int sceneIndex) {
        GameIniciator.Instance.LoadScenes(sceneIndex);
    }


    public void ExitGame() {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
