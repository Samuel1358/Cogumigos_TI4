using System;
using UnityEngine;

public class TrampoloCoguSpot : CoguInteractable
{
    [SerializeField] private GameObject _trampoloPrefab;
    private bool _canActive;

    private void Awake() {
        _trampoloPrefab.SetActive(false);
        _canActive = true;
    }

    public override Action Interact(Cogu cogu) {
        if (_canActive) {
            _trampoloPrefab.SetActive(true);
            _canActive = false;
            return () => { Destroy(cogu.gameObject); };
        }
        return () => {};
    }

    public override Action TEST_Interact(TEST_Cogu cogu)
    {
        if (_canActive)
        {
            _trampoloPrefab.SetActive(true);
            _canActive = false;
            return () => { Destroy(cogu.gameObject); };
        }
        return () => { };
    }

    public override void ResetObject() {
        base.ResetObject();

        _trampoloPrefab.SetActive(false);
        _canActive = true;
    }
}
