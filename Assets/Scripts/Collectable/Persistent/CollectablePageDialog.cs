using DG.Tweening;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class CollectablePageDialog : CollectablePersistenceBase {
    [SerializeField] private GameObject _visual;
    [SerializeField] private GameObject _visualInfo;
    [SerializeField] private bool _autoPlay = true;

    private bool _hasInteractedOnce = false;

    protected override void SetCollectableInactive() {
        _visual.SetActive(false);
        _wasCollected = true;
        GameIniciator.Instance.CanvasIniciatorInstance.InventoryCanvas.UpdateCollectableCountUI();
    }

    public void StartDialog() {
        if (CollectableSO == null || GameIniciator.Instance.DialogManagerInstance.IsDialogActive)
            return;

        if (CollectableSO.RandomLine)
            GameIniciator.Instance.DialogManagerInstance.StartDialog(CollectableSO, Random.Range(0, CollectableSO.LineCount));
        else
            GameIniciator.Instance.DialogManagerInstance.StartDialog(CollectableSO);

        if(GameIniciator.Instance.GameManagerInstance.PlayerInputs != null) GameIniciator.Instance.GameManagerInstance.PlayerInputs.PlayerActions.Interact.started -= StartDialog;

        if (CollectableSO.Duration > 0f) {
            TweenHandler.Timer(CollectableSO.Duration).OnComplete(GameIniciator.Instance.DialogManagerInstance.EndDialog);
            return;
        }

        if (GameIniciator.Instance.GameManagerInstance.PlayerInputs != null) GameIniciator.Instance.GameManagerInstance.PlayerInputs.PlayerActions.Interact.started += AdvanceDialog;
    }

    private void SetVisualActive(bool value) {
        if (_visualInfo == null)
            return;

        _visualInfo.SetActive(value);
    }

    private void AdvanceDialog() {
        if (!GameIniciator.Instance.DialogManagerInstance.IsDialogActive)
            return;

        if (GameIniciator.Instance.DialogManagerInstance.AdvanceDialog())
            if (GameIniciator.Instance.GameManagerInstance.PlayerInputs != null) GameIniciator.Instance.GameManagerInstance.PlayerInputs.PlayerActions.Interact.started -= AdvanceDialog;
    }


    private void StartDialog(CallbackContext callbackContext) {
        StartDialog();
    }

    private void AdvanceDialog(CallbackContext callbackContext) {
        AdvanceDialog();
    }

    private void OnTriggerEnter(Collider collider) {
        if (!_wasCollected) {
            GameIniciator.Instance.AudioManagerInstance.PlaySFX("Collectable");
            SetCollectableInactive();
            GameIniciator.Instance.PersistenceManagerInstance.SaveGame();
            //CollectablePagesUISingleton.instance.CollectablePagesUi.UpdateIndicie(CollectableSO);
        }

        if (CollectableSO.ShowJustOnce) {
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
