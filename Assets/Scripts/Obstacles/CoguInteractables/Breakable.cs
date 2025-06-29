using System;
using UnityEngine;
using UnityEngine.Events;

public class Breakable : CoguInteractable
{
    [SerializeField] private GameObject _visual;
    [SerializeField] private GameObject _vfx;
    [SerializeField] private UnityEvent _onBreak;
    
    private void Awake() {
        NeedReset = false;
    }

    private void DeactivateWall() {
        GetComponent<Collider>().enabled = false;
        _visual.SetActive(false);
        _vfx.SetActive(true);
        NeedReset = true;
    }
    private void ActivateWall() {
        GetComponent<Collider>().enabled = true;
        _visual.SetActive(true);
        _vfx.SetActive(false);
        NeedReset = false;
    }

    public override void Interact(Cogu cogu) {
        DeactivateWall();
        _onBreak.Invoke();
        Destroy(cogu.gameObject);
        //return () => { Destroy(cogu.gameObject); };
    }

    public override void ResetObject() {
        if (NeedReset) 
        {
            base.ResetObject();
            ActivateWall();
            _isAvailable = true;
        }
    }
}
