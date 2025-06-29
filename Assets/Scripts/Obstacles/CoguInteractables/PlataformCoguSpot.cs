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

    public override void Interact(Cogu cogu) {
        if (_canActive) {
            _plataformPrefab.SetActive(true);
            _canActive = false;
            AudioManager.Instance.PlaySFX(SoundEffectNames.COGU_PLATAFORMA);
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
            _plataformPrefab.SetActive(false);
            _canActive = true;

            _isAvailable = true;
            NeedReset = false;
        }       
    }
}
