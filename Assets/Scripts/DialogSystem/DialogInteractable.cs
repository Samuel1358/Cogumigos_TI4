using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace DialogSystem
{
    public class DialogInteractable : MonoBehaviour
    {
        private PlayerInputActions _inputActions;

        [SerializeField] private DialogData dialogData;
        [SerializeField] private GameObject _visualInfo;
        [SerializeField] private bool autoPlay = true;

        private void OnEnable()
        {
            _visualInfo.SetActive(false);

            _inputActions = new PlayerInputActions();
            _inputActions.Player.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Player.Interact.started -= StartDialog;

            _inputActions.Player.Disable();
        }

        // Public Methods
        public void StartDialog()
        {
            if (dialogData == null)
                return;

            if (DialogController.instance.IsDialogActive)
                return;

            DialogController.instance.StartDialog(dialogData);

            _inputActions.Player.Interact.started -= StartDialog;
            _inputActions.Player.Interact.started += AdvanceDialog;
        }


        // Private Methods
        private void AdvanceDialog()
        {
            if (!DialogController.instance.IsDialogActive)
                return;

            if (DialogController.instance.AdvanceDialog())
                _inputActions.Player.Interact.started -= AdvanceDialog;
        }

        // Input
        private void StartDialog(CallbackContext callbackContext)
        {
            StartDialog();
        }

        private void AdvanceDialog(CallbackContext callbackContext)
        {
            AdvanceDialog();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (autoPlay)
            {
                StartDialog();
                return;
            }

            _visualInfo.SetActive(true);
            _inputActions.Player.Interact.started += StartDialog;
        }
        
        private void OnTriggerExit(Collider other)
        {
            _visualInfo.SetActive(false);
            _inputActions.Player.Interact.started -= StartDialog;
        }       
    }
} 