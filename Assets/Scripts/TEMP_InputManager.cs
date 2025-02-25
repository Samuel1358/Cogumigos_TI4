using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TEMP_InputManager : MonoBehaviour
{
    [Header("External Access")]
    [SerializeField] private TEMP_PlayerThirdPerson playerThirdPerson;
    [SerializeField] private TEMP_PlayerMovement playerMovement;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private CameraController cameraController;

    [Header("Keybinds")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    private bool isFreeLookActive = false;

    private void Update()
    {
        // Player Movement
        playerMovement.MovementUpdate(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        playerMovement.JumpUpdate(Input.GetKey(jumpKey));

        // Player Third Person
        if (!isFreeLookActive)
        {
            playerThirdPerson.CameraUpdate(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }

        // Camera Zoom
        if (Input.mouseScrollDelta.y != 0)
        {
            cameraController.UpdateFOV(Input.mouseScrollDelta.normalized.y);
        }
    }

    private void FixedUpdate()
    {
        // Player Movement
        playerMovement.PlayerMove();
    }

    private void LateUpdate()
    {
        // Câmera dinâmica
        if (Input.GetMouseButtonDown(1))
        {
            isFreeLookActive = true;
            cameraController.MoveCamera();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isFreeLookActive = false;
            cameraController.SetOffset(playerTransform.position);
            cameraController.LockedCamera(); 
        }
    }
}
