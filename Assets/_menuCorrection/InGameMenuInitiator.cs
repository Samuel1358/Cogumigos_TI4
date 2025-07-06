using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class InGameMenuInitiator : MonoBehaviour {
    [field: SerializeField] public static InGameMenuInitiator Instance { get; private set; }
    [field: SerializeField] public CheatsUI CheatCanvas { get; private set; }
    [field: SerializeField] public UiInventory InventoryCanvas { get; private set; }
    [field: SerializeField] public GameObject PauseCanvas { get; private set; }
    [field: SerializeField] public GameObject[] InGameCanvasObjects { get; private set; }

    private void Awake() {
        Instance = this;
    }

    public void PauseGameUI() {
        PauseCanvas.SetActive(true);
        GameIniciator.Instance.GameManagerInstance.UnhideMouse();
    }

    public void UnPauseGame() {
        GameIniciator.Instance.UnpauseGame();
    }
    public void UnPauseGameUI() {
        PauseCanvas.SetActive(false);
        GameIniciator.Instance.GameManagerInstance.HideMouse();
        foreach(GameObject obj in InGameCanvasObjects) {
            obj.SetActive(false);
        }
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
