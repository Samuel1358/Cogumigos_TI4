using System;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : CoguInteractable
{
    [SerializeField] private GameObject _fracturedPrefab;
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _explosionRadius;
    private List<Transform> _parts;

    private void Awake() {
        _parts = new List<Transform>();
        NeedReset = false;
    }

    private void DeactivateWall() {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        NeedReset = true;
    }
    private void ActivateWall() {
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
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
            _parts.Add(t.gameObject.transform);
            t.AddExplosionForce(_explosionForce, cogu.transform.position, _explosionRadius);
        }
        return () => { Destroy(cogu.gameObject); };
    }

    public override void ResetObject() {
        base.ResetObject();
        if (NeedReset) 
        { 
            ActivateWall();
            _isAvailable = true;
        }
    }
}
