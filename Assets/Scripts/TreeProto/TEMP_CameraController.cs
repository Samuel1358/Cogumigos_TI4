using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine; // Cinemachine 3.0
using UnityEngine.Events;

[RequireComponent(typeof(Camera))]
public class TEMP_CameraController : MonoBehaviour
{
    [Header("External Access")]
    [SerializeField] private CameraObstacleHandler obstacleHandler;
    [SerializeField] private CinemachineCamera gameCamera;
    [SerializeField] private CinemachineInputAxisController inputController;

    [Header("Zoom")]
    [SerializeField] private float zoomRate;
    [SerializeField, Min(50)] private float zoomMaxDis = 50;
    [SerializeField, Min(10)] private float zoomMinDis = 10;

    private float fieldOfView;

    private void Awake()
    {
    }

    private void Start()
    {
        LockCamera();
        fieldOfView = Camera.main.fieldOfView;
    }

    public void LockCamera()
    {
        inputController.enabled = false;
    }

    public void MoveCamera()
    {
        inputController.enabled = true;
    }


    public void UpdateFOV(float scrollInput)
    {
        fieldOfView += zoomRate * -scrollInput * 0.1f;
        fieldOfView = Mathf.Clamp(fieldOfView, zoomMinDis, zoomMaxDis);

        gameCamera.Lens.FieldOfView = fieldOfView;
    }
}
