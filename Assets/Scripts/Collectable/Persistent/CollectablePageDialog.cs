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
        DataPersistenceManager.Instance.SaveGame();
        UiInventory.Instance.UpdateCollectableCounter();
    }

    public void StartDialog() {
        if (CollectableSO == null || DialogController.instance.IsDialogActive)
            return;

        if (CollectableSO.RandomLine)
            DialogController.instance.StartDialog(CollectableSO, Random.Range(0, CollectableSO.LineCount));
        else
            DialogController.instance.StartDialog(CollectableSO);

        if(GameManager.Instance.PlayerInputs != null) GameManager.Instance.PlayerInputs.PlayerActions.Interact.started -= StartDialog;

        if (CollectableSO.Duration > 0f) {
            TweenHandler.Timer(CollectableSO.Duration).OnComplete(DialogController.instance.EndDialog);
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
            //UiInventory.Instance.UpdateCollectableCountUI(NUMERO DE PAGINAS COLETADAS);
            SetCollectableInactive();
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
