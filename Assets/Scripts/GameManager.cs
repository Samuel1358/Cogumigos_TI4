using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerInput PlayerInputs { get; private set; }
    [SerializeField] private GameObject _frameDebugger;
    [SerializeField] private Player _player;

    public Player Player { get { return _player; } }

    public void WakeUp(GameObject frameDebugger) {
        _frameDebugger = frameDebugger;    
        _frameDebugger.SetActive(false);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F11)) {
            _frameDebugger.SetActive(!_frameDebugger.activeInHierarchy);
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
    public void SetInput(PlayerInput newInput) {
        PlayerInputs = newInput;
    }
    public void SetPlayer(Player scenePlayer) {
        _player = scenePlayer;
    }
}
