using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine; // Cinemachine 3.0
using UnityEngine.Events;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Header("External Access")]
    [SerializeField] private CinemachineCamera unlockedCamera;
    [SerializeField] private CinemachineCamera lockedCamera;
    [SerializeField] private CinemachineInputAxisController test;

    [Header("Zoom")]
    [SerializeField] private float zoomRate;
    [SerializeField, Min(50)] private float zoomMaxDis = 50;
    [SerializeField, Min(10)] private float zoomMinDis = 10;

    private Vector3 offset;
    private float fieldOfView;

    private void Awake()
    {
        offset = transform.position;
    }

    private void Start()
    {
        LockedCamera();
        fieldOfView = Camera.main.fieldOfView;
    }

    public void LockedCamera()
    {

        test.enabled = !test.enabled;
        //unlockedCamera.Priority = 0;
        //lockedCamera.Priority = 10;
    }

    public void MoveCamera()
    {
        test.enabled = !test.enabled;
        //unlockedCamera.Priority = 10;
        //lockedCamera.Priority = 0;
    }


    public void SetOffset(Vector3 targetPosition)
    {
        lockedCamera.transform.position = unlockedCamera.transform.position;
        lockedCamera.transform.rotation = unlockedCamera.transform.rotation;


        /*
        CinemachineOrbitalFollow lockedFollowComponent = lockedCamera.GetComponent<CinemachineOrbitalFollow>();
        CinemachineOrbitalFollow unlockedFollowComponent = unlockedCamera.GetComponent<CinemachineOrbitalFollow>();

        if (lockedFollowComponent != null && unlockedFollowComponent != null)
        {
            Debug.Log(lockedFollowComponent);
            Debug.Log(unlockedFollowComponent);
        }
        */


    }

    public void UpdateFOV(float scrollInput)
    {
        fieldOfView += zoomRate * -scrollInput * 0.1f;
        fieldOfView = Mathf.Clamp(fieldOfView, zoomMinDis, zoomMaxDis);

        unlockedCamera.Lens.FieldOfView = fieldOfView;
        lockedCamera.Lens.FieldOfView = fieldOfView;
    }
}
