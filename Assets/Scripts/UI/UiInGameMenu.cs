using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class UiInGameMenu : MonoBehaviour {
    public static UiInGameMenu Instance;
    [SerializeField] private PlayerInput _inputs;
    [SerializeField] private GameObject _pauseScreen;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            HidePauseMenu();
        }
        else {
            Destroy(this);
        }
    }

    public void ChangePauseState() {
        if (_pauseScreen.activeInHierarchy) {
            HidePauseMenu();
        }
        else {
            ShowPauseMenu();
        }
    }

    public void ShowPauseMenu() {
        _pauseScreen.SetActive(true);
        GameManager.Instance?.PauseGame();
    }
    public void HidePauseMenu() {
        _pauseScreen.SetActive(false);
        GameManager.Instance?.UnpauseGame();
    }

    public void ExitGame() {

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ReturnToMenu() {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
