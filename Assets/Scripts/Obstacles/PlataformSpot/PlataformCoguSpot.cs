using System;
using UnityEngine;

public class PlataformCoguSpot : CoguInteractable
{
    [SerializeField] private GameObject _plataformPrefab;
    private bool _canActive;

    private void Awake() {
        _plataformPrefab.SetActive(false);
        _canActive = true;
    }

    public override Action Interact(Cogu cogu) {
        if (_canActive) {
            _plataformPrefab.SetActive(true);
            _canActive = false;

            _isAvailable = false;
            NeedReset = true;
            return () => { Destroy(cogu.gameObject); };
        }
        return () => {};
    }

    public override void ResetObject() 
    {
        if (NeedReset)
        {
            _plataformPrefab.SetActive(false);
            _canActive = true;

            _isAvailable = true;
            NeedReset = false;
        }       
    }
}
