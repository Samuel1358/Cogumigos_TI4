using System.Collections;
using UnityEngine;
using DG.Tweening;

public class ExplosiveBarrel : ResetableBase {
    [SerializeField] private float _timeToExplode;
    [SerializeField] private float _timeToReload;
    [SerializeField] private float _finalScale;
    [SerializeField] private Color _finalColor;
    [SerializeField] private GameObject _visual;
    [SerializeField] private Transform _explosionRadiusVisual;
    private bool _willExplodePlayer;
    private bool _wasExploded;
    private SphereCollider _collider;
    private Material _material;
    private Color _initinalColor;
    private Vector3 _initinalExplosionRadius;

    private void Awake() {
        _wasExploded = false;
        _willExplodePlayer = false;
        _collider = GetComponent<SphereCollider>();
        _material = GetComponentInChildren<MeshRenderer>().material;
        _initinalColor = _material.color;
        _initinalExplosionRadius = _explosionRadiusVisual.localScale;
        _explosionRadiusVisual.localScale = new Vector3(0.01f, 0.01f, 0.01f);
    }

    private void OnTriggerEnter(Collider other) {
        if (!_wasExploded) {
            StartCoroutine(ExplosionAfterColdown());
            _material.DOColor(_finalColor, _timeToExplode);
            transform.DOScale(_finalScale, _timeToExplode);
            _explosionRadiusVisual.DOScale(_initinalExplosionRadius, _timeToExplode);
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
        _collider.enabled = false;
        _wasExploded = true;
        if (_willExplodePlayer) {
            RespawnController.OnPlayerRespawn.Invoke();
        }
        transform.localScale = new Vector3(1f, 1f, 1f);
        _explosionRadiusVisual.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        _material.color = _initinalColor;
        yield return new WaitForSeconds(_timeToReload);
        _visual.SetActive(true);
        _collider.enabled = true;
        _willExplodePlayer = false;
        _wasExploded = false;
    }
}
