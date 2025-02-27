using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
}
