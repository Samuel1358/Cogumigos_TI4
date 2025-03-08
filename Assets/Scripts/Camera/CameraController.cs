using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine; // Cinemachine 3.0
using UnityEngine.Events;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Header("External Access")]
    //[SerializeField] private CinemachineCamera unlockedCamera;
    [SerializeField] private CinemachineCamera cameraObj;
    [SerializeField] private CinemachineInputAxisController inputController;

    [Header("Zoom")]
    [SerializeField] private float zoomRate;
    [SerializeField, Min(50)] private float zoomMaxDis = 50;
    [SerializeField, Min(10)] private float zoomMinDis = 10;

    //private Vector3 offset;
    private float fieldOfView;

    /*private void Awake()
    {
        offset = transform.position;
    }*/

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

        //unlockedCamera.Lens.FieldOfView = fieldOfView;
        cameraObj.Lens.FieldOfView = fieldOfView;
    }
}
