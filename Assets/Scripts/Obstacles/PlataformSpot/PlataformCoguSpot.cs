using System;
using UnityEngine;

public class PlataformCoguSpot : ResetableBase, IInteractable {
    [SerializeField] private GameObject _plataformPrefab;
    private bool _canActive;

    private void Awake() {
        _plataformPrefab.SetActive(false);
        _canActive = true;
    }

    public Action Interact(Cogu cogu) {
        if (_canActive) {
            _plataformPrefab.SetActive(true);
            _canActive = false;
            return () => { Destroy(cogu.gameObject); };
        }
        return () => {};
    }

    public override void ResetObject() {
        base.ResetObject();

        _plataformPrefab.SetActive(false);
        _canActive = true;
    }
}
