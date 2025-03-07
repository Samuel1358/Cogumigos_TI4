using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInput : MonoBehaviour {
    public PlayerInputActions InputActions { get; private set; }
    public PlayerInputActions.PlayerActions PlayerActions { get; private set; }
    private void Awake() {
        InputActions = new PlayerInputActions();
        PlayerActions = InputActions.Player;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void OnEnable() {
        PlayerActions.Enable();
    }
    private void OnDisable() {
        PlayerActions.Disable();
    }
    public void DisableActionFor(InputAction action, float seconds) {
        StartCoroutine(DiableAction(action, seconds));
    }
    private IEnumerator DiableAction(InputAction action, float seconds) {
        action.Disable();
        yield return new WaitForSeconds(seconds);
        action.Enable();
    }
}
