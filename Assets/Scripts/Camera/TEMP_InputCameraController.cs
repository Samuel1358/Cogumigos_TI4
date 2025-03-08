using UnityEngine;

public class TEMP_InputCameraController : MonoBehaviour
{
    [Header("External Access")]
    [SerializeField] private CameraController cameraController;

    private bool isFreeLookActive = false;

    private void Update()
    {
        // Camera Zoom
        if (Input.mouseScrollDelta.y != 0)
        {
            cameraController.UpdateFOV(Input.mouseScrollDelta.normalized.y);
        }
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
            cameraController.LockCamera();
        }
    }
}
