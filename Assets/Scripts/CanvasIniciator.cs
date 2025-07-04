#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasIniciator : MonoBehaviour {
    [field: SerializeField] public GameObject MainMenu { get; private set; }
    [field: SerializeField] public GameObject StageList { get; private set; }
    [field: SerializeField] public GameObject ConfigMenu { get; private set; }
    [field: SerializeField] public GameObject HelpMenu { get; private set; }
    [field: SerializeField] public GameObject CreditsMenu { get; private set; }
    [field: SerializeField] public GameObject InGameMenu { get; private set; }
    [field: SerializeField] public UiInventory InventoryCanvas { get; private set; }
    [field: SerializeField] public GameObject BarkCanvas { get; private set; }
    [field: SerializeField] public GameObject GameOverCanvas { get; private set; }
    [field: SerializeField] public CheatsUI CheatCanvas { get; private set; }

    public void WakeUp() {
        LoadMenuScene();
    }

    public void LoadScene(int sceneIndex) {
        if (sceneIndex == 0) {
            LoadMenuScene();
            GameIniciator.Instance.LoadScenes(sceneIndex);
        }
        else {
            LoadGameScene();
            GameIniciator.Instance.LoadScenes(sceneIndex);
        }
    }
    public void LoadMenuScene() {
        MainMenu.SetActive(true);
        StageList.SetActive(false);
        ConfigMenu.SetActive(false);
        HelpMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        InGameMenu.SetActive(false);
        InventoryCanvas.gameObject.SetActive(false);
        BarkCanvas.SetActive(false);
        GameOverCanvas.SetActive(false);
        CheatCanvas.gameObject.SetActive(false);
    }
    private void LoadGameScene() {
        MainMenu.SetActive(false);
        StageList.SetActive(false);
        ConfigMenu.SetActive(false);
        HelpMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        InGameMenu.SetActive(false);
        InventoryCanvas.gameObject.SetActive(true);
        BarkCanvas.SetActive(false);
        GameOverCanvas.SetActive(false);
        CheatCanvas.gameObject.SetActive(false);
    }
    public void ExitGame() {

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
