using DG.Tweening;
using DialogSystem;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class CollectablePageDialog : CollectablePersistenceBase {
    [SerializeField] private GameObject _visual;
    [SerializeField] private GameObject _visualInfo;
    [SerializeField] private bool _autoPlay = true;

    private bool _hasInteractedOnce = false;

    protected override void SetCollectableInactive() {
        _visual.SetActive(false);
        WasCollected = true;
    }

    public void StartDialog() {
        if (_collectableSO == null || DialogController.instance.IsDialogActive)
            return;

        if (_collectableSO.RandomLine)
            DialogController.instance.StartDialog(_collectableSO, Random.Range(0, _collectableSO.LineCount));
        else
            DialogController.instance.StartDialog(_collectableSO);

        if(GameManager.Instance.PlayerInputs != null) GameManager.Instance.PlayerInputs.PlayerActions.Interact.started -= StartDialog;

        if (_collectableSO.Duration > 0f) {
            TweenHandler.Timer(_collectableSO.Duration).OnComplete(DialogController.instance.EndDialog);
            return;
        }

        if (GameManager.Instance.PlayerInputs != null) GameManager.Instance.PlayerInputs.PlayerActions.Interact.started += AdvanceDialog;
    }

    private void SetVisualActive(bool value) {
        if (_visualInfo == null)
            return;

        _visualInfo.SetActive(value);
    }

    private void AdvanceDialog() {
        if (!DialogController.instance.IsDialogActive)
            return;

        if (DialogController.instance.AdvanceDialog())
            if (GameManager.Instance.PlayerInputs != null) GameManager.Instance.PlayerInputs.PlayerActions.Interact.started -= AdvanceDialog;
    }


    private void StartDialog(CallbackContext callbackContext) {
        StartDialog();
    }

    private void AdvanceDialog(CallbackContext callbackContext) {
        AdvanceDialog();
    }

    private void OnTriggerEnter(Collider collider) {
        if (!WasCollected) {
            AudioManager.Instance.PlaySFX("Collectable");
            SetCollectableInactive();
        }

        if (_collectableSO.ShowJustOnce) {
            if (_hasInteractedOnce)
                return;
            else
                _hasInteractedOnce = true;
        }

        if (_autoPlay) {
            StartDialog();
            return;
        }

        SetVisualActive(true);
        SetCollectableInactive();
    }

    private void OnTriggerExit(Collider other) {
        SetVisualActive(false);
    }

}
