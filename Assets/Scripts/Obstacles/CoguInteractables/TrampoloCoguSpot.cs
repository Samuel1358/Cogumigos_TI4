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

    public override void Interact(Cogu cogu) {
        if (_canActive) {
            _trampoloPrefab.SetActive(true);
            _canActive = false;
            GameIniciator.Instance.AudioManagerInstance.PlaySFX(SoundEffectNames.COGU_COZINHEIRO);
            _isAvailable = false;
            NeedReset = true;
            Destroy(cogu.gameObject);
            //return () => { Destroy(cogu.gameObject); };
        }
        //return () => {};
    }

    public override void ResetObject() 
    {        
        if (NeedReset)
        {
            base.ResetObject();
            _trampoloPrefab.SetActive(false);
            _canActive = true;

            _isAvailable = true;
            NeedReset = false;
        }       
    }
}
