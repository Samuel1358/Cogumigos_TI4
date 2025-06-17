using UnityEngine;

public class BillboardUI : MonoBehaviour
{
    private Transform mainCamera;

    void Start()
    {
        mainCamera = Camera.main.transform;
    }

    void LateUpdate()
    {
        Vector3 lookDirection = transform.position + mainCamera.forward;
        transform.LookAt(lookDirection, Vector3.up);
    }
}
