using System;
using UnityEngine;

public class TEMP_InputManager : MonoBehaviour
{
    [HideInInspector] public static TEMP_InputManager instance;

    [Header("External Access")]
    //[SerializeField] private TargetCursor targetCursor;
    //[SerializeField] private FriendshroomManager friendshroomManager;
    [SerializeField] private CameraController cameraController;
    //[SerializeField] private TEMP_UIController uiController;

    //private bool isFreeLookActive = false;

    // Events
    public Action onInteractInput;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        // Camera Zoom
        if (Input.mouseScrollDelta.y != 0)
        {
            cameraController.UpdateFOV(Input.mouseScrollDelta.normalized.y);
        }

        // Target Cursor
        //targetCursor.UpdatePosition(Input.mousePosition);

        // UIController
        // Ignorar est� mudan�a para as outras branches
        /*if (Input.GetKeyDown(KeyCode.Escape))
        {
#pragma warning disable CS0618 // 'GameObject.active' � obsoleto
            if (uiController.painel.active)
                uiController.Desable();
            else
                uiController.Active();
        }*/

        if (Input.GetKeyDown(KeyCode.E))
        {           
            onInteractInput?.Invoke();
        }
    }

    private void LateUpdate()
    {
        // C�mera din�mica
        // if (Input.GetMouseButtonDown(1))
        // {
            //isFreeLookActive = true;
            cameraController.MoveCamera();
        // }
        // else if (Input.GetMouseButtonUp(1))
        //{
            //isFreeLookActive = false;
            //cameraController.LockCamera();
        //}
    }
}
