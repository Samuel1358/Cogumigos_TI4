using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace DialogSystem
{
    public class DialogInteractable : MonoBehaviour
    {
        private PlayerInputActions _inputActions;

        [SerializeField] private DialogData dialogData;
        [SerializeField] private bool autoPlay = false;
        [SerializeField] private KeyCode interactionKey = KeyCode.E;
        
        private DialogController dialogController;
        private bool playerInRange;

        private void OnEnable()
        {
            _inputActions = new PlayerInputActions();
            _inputActions.Player.Enable();

            _inputActions.Player.Interact.started += StartDialog;
        }

        private void OnDisable()
        {
            _inputActions.Player.Interact.started -= StartDialog;

            _inputActions.Player.Disable();
        }

        private void Start()
        {
            dialogController = FindFirstObjectByType<DialogController>();
            
            if (autoPlay)
            {
                if (dialogData == null || dialogController == null)
                {
                    return;
                }

                dialogController.StartDialog(dialogData);
            }
        }
        
        private void Update()
        {
            if (dialogController == null)
            {
                return;
            }
            
            if (dialogController.IsDialogActive && Input.GetKeyDown(interactionKey))
            {
                dialogController.AdvanceDialog();
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                playerInRange = true;
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                playerInRange = false;
            }
        }
        
        public void StartDialog(CallbackContext callbackContext)
        {
            if (dialogData == null || dialogController == null)
            {
                return;
            }

            if (!playerInRange && dialogController.IsDialogActive)
            {
                return;
            }
            
            dialogController.StartDialog(dialogData);
        }
    }
} 