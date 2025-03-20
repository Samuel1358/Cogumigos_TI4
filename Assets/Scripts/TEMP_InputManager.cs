using UnityEngine;

public class TEMP_InputManager : MonoBehaviour
{
    [HideInInspector] public static TEMP_InputManager instance;

    [Header("External Access")]
    [SerializeField] private TargetCursor targetCursor;
    //[SerializeField] private FriendshroomManager friendshroomManager;
    [SerializeField] private CameraController cameraController;

    //private bool isFreeLookActive = false;

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
        targetCursor.UpdatePosition(Input.mousePosition);
        targetCursor.AttractFriendshroom(Input.GetKey(KeyCode.F));

        // Friendshroom Manager
        CoguManager.instance.ThrowFriendshroom(Input.GetMouseButtonDown(0));

        CoguManager.instance.DisbandArmy(Input.GetKeyDown(KeyCode.R));
        CoguManager.instance.ChangeArmySelectedType(Input.GetKeyDown(KeyCode.Tab));
    }

    private void LateUpdate()
    {
        // Câmera dinâmica
        if (Input.GetMouseButtonDown(1))
        {
            //isFreeLookActive = true;
            cameraController.MoveCamera();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            //isFreeLookActive = false;
            cameraController.LockCamera();
        }
    }
}
