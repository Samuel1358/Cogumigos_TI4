using System;
using UnityEngine;

public class TEMP_InputManager : MonoBehaviour
{
    public static TEMP_InputManager instance;

    [Header("External Access")]
 
    public Action onInteractInput;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {           
            onInteractInput?.Invoke();
        }
    }
}
