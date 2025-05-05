using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Rendering;

public class Breakable : CoguInteractable
{
    [SerializeField] private GameObject _fracturedPrefab;
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
            t.AddExplosionForce(cogu.GetCoguData().ExplosionForce, cogu.transform.position, cogu.GetCoguData().ExplosionRadius);
        }
        return () => { Destroy(cogu.gameObject); };
    }

    public override Action TEST_Interact(TEST_Cogu cogu)
    {
        DeactivateWall();
        Transform father = Instantiate(_fracturedPrefab, transform).transform;
        Rigidbody[] aux = father.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody t in aux)
        {
            _parts.Add(t.gameObject.transform);
            //t.AddExplosionForce(cogu.GetCoguData().ExplosionForce, cogu.transform.position, cogu.GetCoguData().ExplosionRadius);
        }
        return () => { Destroy(cogu.gameObject); };
    }

    public override void ResetObject() {
        base.ResetObject();
        if (NeedReset) ActivateWall();
    }
}
