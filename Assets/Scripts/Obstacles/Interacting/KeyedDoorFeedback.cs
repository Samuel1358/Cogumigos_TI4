using System;
using UnityEngine;
using DG.Tweening;

public class KeyedDoorFeedback : ResetableBase
{
    [Header("External Accesses")]
    [SerializeField] private Transform _lockSpot;
    [SerializeField] private GameObject _keyVisual;
    //[SerializeField, Min(0.1f)] private float _keyVisualScale;

    [Header("Settings")]
    [SerializeField] private Ease _easeBase = Ease.Linear;
    [SerializeField] private float _moveDuration;
    [SerializeField] private float _rotateOnMoveDuration;
    [SerializeField] private float _finalRotateSpeed;
    [SerializeField] private float _finalRotateDuration;

    private bool _activeAtCheckpoint;

    private void Start()
    {
        _keyVisual.SetActive(false);
        //_keyVisual.transform.localScale *= _keyVisualScale;

        RespawnController.OnPlayerChangeCheckPoint += SetMemory;
    }

    public void Open(Collider other, Action openAction)
    {
        _keyVisual.transform.position = other.transform.position;
        _keyVisual.transform.rotation = Quaternion.identity;
        _keyVisual.SetActive(true);

        if (_rotateOnMoveDuration > _moveDuration)
            _rotateOnMoveDuration = _moveDuration;

        Tween move = DOTween.To(() => _keyVisual.transform.position, x => _keyVisual.transform.position = x, _lockSpot.position, _moveDuration).SetEase(_easeBase);
        Tween rotateOnMove = DOTween.To(() => _keyVisual.transform.localEulerAngles, x => _keyVisual.transform.localEulerAngles = x, _lockSpot.localEulerAngles, _rotateOnMoveDuration).SetEase(_easeBase);

        Tween finalRotate = TweenHandler.Timer(_finalRotateDuration)
            .OnUpdate(() => FinalRotate(_keyVisual.transform));            

        Sequence sequence = DOTween.Sequence()
            .Append(move)
            .Append(finalRotate)
            .OnComplete(openAction.Invoke);
    }

    private void FinalRotate(Transform transform)
    {
        transform.Rotate(_finalRotateSpeed * Time.deltaTime * Vector3.up, Space.Self);
    }  

    // Resetable
    private void SetMemory(Checkpoint checkpoint)
    {
        _activeAtCheckpoint = _keyVisual.activeSelf;
    }

    public override void ResetObject()
    {
        _keyVisual.SetActive(_activeAtCheckpoint);
    }
}
