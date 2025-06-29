using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameObject FrameDebugger;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            FrameDebugger.SetActive(false);
        }
        else {
            Destroy(this);
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F11)) {
            FrameDebugger.SetActive(!FrameDebugger.activeInHierarchy);
        }
    }

    public void PauseGame() {
        Time.timeScale = 0f;
        PlayerInput.UnhideAndUnlockMouse();
    }
    public void UnpauseGame() {
        Time.timeScale = 1f;
        PlayerInput.HideAndLockMouse();
    }
}
