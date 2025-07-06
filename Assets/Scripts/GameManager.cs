using UnityEditor.ShaderGraph.Serialization;
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

    public void HideMouse() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void UnhideMouse() {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void SetInput(PlayerInput newInput) {
        PlayerInputs = newInput;
    }
    public void SetPlayer(Player scenePlayer) {
        _player = scenePlayer;
    }
}
