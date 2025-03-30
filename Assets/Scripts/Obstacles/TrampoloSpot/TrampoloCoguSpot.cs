using System;
using UnityEngine;

public class TrampoloCoguSpot : MonoBehaviour, IResetable, IInteractable {
    [SerializeField] private Checkpoint _linkedCheckpoint;
    [SerializeField] private GameObject _trampoloPrefab;
    private bool _canActive;

    private void Awake() {
        _trampoloPrefab.SetActive(false);
        _canActive = true;
    }
    private void OnEnable() {
        RespawnController.OnPlayerChangeCheckPoint += VerifyReset;
    }

    private void OnDisable() {
        RespawnController.OnPlayerChangeCheckPoint -= VerifyReset;
        RespawnController.Instance.TurnTrapNonResetable(this);
    }

    public Action Interact(Cogu cogu) {
        if (_canActive) {
            _trampoloPrefab.SetActive(true);
            _canActive = false;
            return () => { Destroy(cogu.gameObject); };
        }
        return () => {};
    }
    private void VerifyReset(Checkpoint checkpoint) {
        if (RespawnController.Instance.PlayerLastCheckPoint == null) {
            RespawnController.Instance.TurnTrapResetable(this);
            return;
        }
        if (RespawnController.Instance.PlayerLastCheckPoint == _linkedCheckpoint) {
            RespawnController.Instance.TurnTrapNonResetable(this);
        }
    }

    public void ResetTrap() {
        _trampoloPrefab.SetActive(false);
        _canActive = true;
    }
}
