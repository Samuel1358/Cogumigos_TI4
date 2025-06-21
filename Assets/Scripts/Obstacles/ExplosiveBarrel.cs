using System.Collections;
using UnityEngine;

public class ExplosiveBarrel : ResetableBase {
    [SerializeField] private float _timeToExplode;
    [SerializeField] private GameObject _visual;
    private bool _willExplodePlayer;
    private bool _wasExploded;

    private void Awake() {
        _wasExploded = false;
        _willExplodePlayer = false;
    }

    private void OnTriggerEnter(Collider other) {
        if (!_wasExploded) {
            StartCoroutine(ExplosionAfterColdown());
            _willExplodePlayer = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (!_wasExploded) {
            _willExplodePlayer = false;
        }
    }

    public override void ResetObject() {
        //base.ResetObject();

        _visual.SetActive(true);
        _willExplodePlayer = false;
        _wasExploded = false;
    }

    IEnumerator ExplosionAfterColdown() {
        yield return new WaitForSeconds(_timeToExplode);
        _visual.SetActive(false);
        if (_willExplodePlayer) {
            RespawnController.OnPlayerRespawn.Invoke();
        }
    }
}
