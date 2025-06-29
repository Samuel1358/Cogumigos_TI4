using System;
using UnityEngine;
using UnityEngine.Events;

public class Breakable : CoguInteractable
{
    [SerializeField] private GameObject _visual;
    [SerializeField] private UnityEvent _onBreak;
    
    private void Awake() {
        NeedReset = false;
    }

    private void DeactivateWall() {
        GetComponent<Collider>().enabled = false;
        _visual.SetActive(false);
        NeedReset = true;
    }
    private void ActivateWall() {
        GetComponent<Collider>().enabled = true;
        _visual.SetActive(true);
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
