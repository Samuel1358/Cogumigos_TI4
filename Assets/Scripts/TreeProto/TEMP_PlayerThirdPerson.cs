using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    public class TEMP_PlayerThirdPerson : MonoBehaviour
    {
        [Header("External Access")]
        [SerializeField] private Transform orientation;
        [SerializeField] private Transform playerModel;

        [Header("Rotation")]
        [SerializeField] private float rotationSpd;

        private void Start()
        {
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;

            //Debug.Log("Cursor locked");
        }

        // Metodos Publicos
        public void CameraUpdate(float horizontalInput, float verticalInput)
        {
            Vector3 viewDir = transform.position - new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z);
            orientation.forward = viewDir.normalized;

            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
                playerModel.forward = Vector3.Slerp(playerModel.forward, inputDir.normalized, Time.deltaTime * rotationSpd);
        }
    }
