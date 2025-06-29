using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInput : MonoBehaviour {
    public PlayerInputActions InputActions { get; private set; }
    public PlayerInputActions.PlayerActions PlayerActions { get; private set; }
    private void Awake() {
        InputActions = new PlayerInputActions();
        PlayerActions = InputActions.Player;
        HideAndLockMouse();
    }

    public static void HideAndLockMouse() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public static void UnhideAndUnlockMouse() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnEnable() {
        PlayerActions.Enable();
        GameManager.Instance?.SetInput(this);
        PlayerActions.Pause.canceled += OnPauseCanceled;
    }

    private void OnDisable() {
        PlayerActions.Pause.canceled -= OnPauseCanceled;
        PlayerActions.Disable();
    }

    private void OnPauseCanceled(InputAction.CallbackContext obj) {
        UiInGameMenu.Instance?.ChangePauseState();
    }
    public void DisableActionFor(InputAction action, float seconds) {
        StartCoroutine(DisableAction(action, seconds));
    }
    private IEnumerator DisableAction(InputAction action, float seconds) {
        action.Disable();
        yield return new WaitForSeconds(seconds);
        action.Enable();
    }
}
