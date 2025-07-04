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
        SetKeyVisualActive(false);
        //_keyVisual.transform.localScale *= _keyVisualScale;

        GameIniciator.Instance.RespawnControllerInstance.OnPlayerChangeCheckPoint += SetMemory;
    }

    public void Open(Collider other, Action openAction)
    {
        if (_keyVisual == null)
        {
            openAction.Invoke();
            return;
        }

        GameIniciator.Instance.AudioManagerInstance.PlaySFX(SoundEffectNames.CHAVE_PORTA);

        _keyVisual.transform.position = other.transform.position;
        _keyVisual.transform.rotation = Quaternion.identity;
        SetKeyVisualActive(true);

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

    private void SetKeyVisualActive(bool value)
    {
        if (_keyVisual != null)
            _keyVisual.SetActive(value);
    }

    // Resetable
    private void SetMemory(Checkpoint checkpoint)
    {
        if (_keyVisual == null)
            return;

        _activeAtCheckpoint = _keyVisual.activeSelf;
    }

    public override void ResetObject()
    {
        SetKeyVisualActive(_activeAtCheckpoint);
    }
}
