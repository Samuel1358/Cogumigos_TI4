using System;
using UnityEngine;

public class PlataformCoguSpot : MonoBehaviour, IResetable, IInteractable {
    [SerializeField] private Checkpoint _linkedCheckpoint;
    [SerializeField] private GameObject _plataformPrefab;
    private bool _canActive;

    private void Awake() {
        _plataformPrefab.SetActive(false);
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
            _plataformPrefab.SetActive(true);
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
        _plataformPrefab.SetActive(false);
        _canActive = true;
    }
}
