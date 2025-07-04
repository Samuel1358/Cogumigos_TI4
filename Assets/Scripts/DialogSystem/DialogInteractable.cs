using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using DG.Tweening;

namespace DialogSystem
{
    public class DialogInteractable : MonoBehaviour
    {
        private PlayerInputActions _inputActions;

        [SerializeField] private DialogData _dialogData;
        [SerializeField] private GameObject _visualInfo;
        [SerializeField] private bool _autoPlay = true;

        private bool _hasInteractedOnce = false;

        private void OnEnable()
        {
            SetVisualActive(false);

            _inputActions = new PlayerInputActions();
            _inputActions.Player.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Player.Interact.started -= StartDialog;

            _inputActions.Player.Disable();
        }

        #region // Public Methods

        public void StartDialog()
        {
            if (_dialogData == null || GameIniciator.Instance.DialogManagerInstance.IsDialogActive)
                return;

            //start dialog
            if (_dialogData.RandomLine)
                GameIniciator.Instance.DialogManagerInstance.StartDialog(_dialogData, Random.Range(0, _dialogData.LineCount));
            else
                GameIniciator.Instance.DialogManagerInstance.StartDialog(_dialogData);

            _inputActions.Player.Interact.started -= StartDialog;

            //line auto-close
            if (_dialogData.Duration > 0f)
            {
                TweenHandler.Timer(_dialogData.Duration).OnComplete(GameIniciator.Instance.DialogManagerInstance.EndDialog);
                return;
            }

            _inputActions.Player.Interact.started += AdvanceDialog;
        }

        #endregion

        #region // Private Methods

        private void SetVisualActive(bool value)
        {
            if (_visualInfo == null)
                return;

            _visualInfo.SetActive(value);
        }

        private void AdvanceDialog()
        {
            if (!GameIniciator.Instance.DialogManagerInstance.IsDialogActive)
                return;

            if (GameIniciator.Instance.DialogManagerInstance.AdvanceDialog())
                _inputActions.Player.Interact.started -= AdvanceDialog;
        }

        #endregion

        #region // Inputs

        private void StartDialog(CallbackContext callbackContext)
        {
            StartDialog();
        }

        private void AdvanceDialog(CallbackContext callbackContext)
        {
            AdvanceDialog();
        }

        #endregion

        #region // Trigger

        private void OnTriggerEnter(Collider other)
        {
            //verify just once interact
            if (_dialogData.ShowJustOnce)
            {
                if (_hasInteractedOnce)
                    return;
                else
                    _hasInteractedOnce = true;
            }

            // auto-play
            if (_autoPlay)
            {
                StartDialog();
                return;
            }

            SetVisualActive(true);
            _inputActions.Player.Interact.started += StartDialog;
        }
        
        private void OnTriggerExit(Collider other)
        {
            SetVisualActive(false);
            _inputActions.Player.Interact.started -= StartDialog;
        }

        #endregion
    }
} 