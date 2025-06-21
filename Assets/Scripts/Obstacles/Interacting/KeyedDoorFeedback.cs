using System;
using UnityEngine;
using DG.Tweening;

public class KeyedDoorFeedback : MonoBehaviour
{
    [Header("External Accesses")]
    [SerializeField] private Transform _lockSpot;
    [SerializeField] private GameObject _keyModel;
    [SerializeField, Min(0.1f)] private float _keyModelScale;

    [Header("Settings")]
    [SerializeField] private Ease _easeBase = Ease.Linear;
    [SerializeField] private float _moveDuration;
    [SerializeField] private float _rotateOnMoveDuration;
    [SerializeField] private float _finalRotateSpeed;
    [SerializeField] private float _finalRotateDuration;

    public void Open(Collider other, Action openAction)
    {
        GameObject key = Instantiate(_keyModel, other.transform.position, Quaternion.identity);
        key.transform.localScale *= _keyModelScale;

        if (_rotateOnMoveDuration > _moveDuration)
            _rotateOnMoveDuration = _moveDuration;

        Tween move = DOTween.To(() => key.transform.position, x => key.transform.position = x, _lockSpot.position, _moveDuration).SetEase(_easeBase);
        Tween rotateOnMove = DOTween.To(() => key.transform.localEulerAngles, x => key.transform.localEulerAngles = x, _lockSpot.eulerAngles, _rotateOnMoveDuration).SetEase(_easeBase);

        Tween finalRotate = TweenHandler.Timer(_finalRotateDuration)
            .OnUpdate(() => FinalRotate(key.transform));            

        Sequence sequence = DOTween.Sequence()
            .Append(move)
            .Append(finalRotate)
            .OnComplete(openAction.Invoke);
    }

    private void FinalRotate(Transform transform)
    {
        transform.Rotate(_finalRotateSpeed * Time.deltaTime * Vector3.up, Space.Self);
    }
}
