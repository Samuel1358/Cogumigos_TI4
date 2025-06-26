using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Breakable : CoguInteractable
{
    [SerializeField] private GameObject _visual;
    [SerializeField] private GameObject _fracturedPrefab;
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _explosionRadius;
    private List<Transform> _parts;

    [SerializeField] private UnityEvent _onBreak;
    
    private void Awake() {
        _parts = new List<Transform>();
        NeedReset = false;
    }

    private void DeactivateWall() {
        //GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        _visual.SetActive(false);
        NeedReset = true;
    }
    private void ActivateWall() {
        //GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
        _visual.SetActive(true);
        foreach (Transform t in _parts) {
            Destroy(t.gameObject);
        }
        _parts = new List<Transform>();
        NeedReset = false;
    }

    public override Action Interact(Cogu cogu) {
        DeactivateWall();
        Transform father = Instantiate(_fracturedPrefab, transform).transform;
        Rigidbody[] aux = father.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody t in aux) {
            t.AddExplosionForce(_explosionForce, cogu.transform.position, _explosionRadius);
        }
        _parts.Add(father);
        _onBreak.Invoke();
        return () => { Destroy(cogu.gameObject); };
    }

    public override void ResetObject() {
        //base.ResetObject();
        if (NeedReset) 
        { 
            ActivateWall();
            _isAvailable = true;
        }
    }
}
