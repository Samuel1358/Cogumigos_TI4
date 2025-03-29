using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Rendering;

public class Breakable : MonoBehaviour, IResetable, IInteractable {
    [SerializeField] private GameObject _fracturedPrefab;
    [SerializeField] private Checkpoint _linkedCheckpoint;
    private List<Transform> _parts;
    private bool _needReset;
    private void Awake() {
        _parts = new List<Transform>();
        _needReset = false;
    }
    private void OnEnable() {
        RespawnController.OnPlayerChangeCheckPoint += VerifyReset;
    }

    private void OnDisable() {
        RespawnController.OnPlayerChangeCheckPoint -= VerifyReset;
        RespawnController.Instance.TurnTrapNonResetable(this);
    }

    private void DeactivateWall() {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        _needReset = true;
    }
    private void ActivateWall() {
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
        foreach (Transform t in _parts) {
            Destroy(t.gameObject);
        }
        _parts = new List<Transform>();
        _needReset = false;
    }

    public void ResetTrap() {
        if (_needReset) ActivateWall();
    }

    private void VerifyReset(Checkpoint checkpoint) {
        if (RespawnController.Instance.PlayerLastCheckPoint == null) {
            RespawnController.Instance.TurnTrapResetable(this);
            return;
        }
        if (RespawnController.Instance.PlayerLastCheckPoint == _linkedCheckpoint) {
            RespawnController.Instance.TurnTrapNonResetable(this);
        }
    }

    public Action Interact(Cogu cogu) {
        DeactivateWall();
        Transform father = Instantiate(_fracturedPrefab, transform).transform;
        Rigidbody[] aux = father.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody t in aux) {
            _parts.Add(t.gameObject.transform);
            t.AddExplosionForce(cogu.GetCoguData().ExplosionForce, cogu.transform.position, cogu.GetCoguData().ExplosionRadius);
        }
        return () => { Destroy(cogu.gameObject); };
    }
}
