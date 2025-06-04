using UnityEngine;
using DG.Tweening;
using System;

public class DoTweenUtility : MonoBehaviour
{
    public static DoTweenUtility instance;

    [SerializeField] private float _shakeStrenght = 2f;
    [SerializeField] private int _shakeVibrato = 5;
    [SerializeField, Range(0f, 90f)] private float _randomness = 0f;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
    }

    public void Timer(float time, Action actionBefore)
    {
        float count = 0;
        DOTween.To(() => count, x => count = x, 10, time).OnComplete(() => actionBefore.Invoke());
    }

    public void FallingPlatformShake(Transform transform, float time, Vector3 shakeDirection, Action actionBefore)
    {
        DOTween.Shake(() => transform.localRotation.eulerAngles, 
            x => transform.localRotation = Quaternion.Euler(x), 
            time, shakeDirection.normalized * _shakeStrenght, 
            _shakeVibrato, _randomness).OnComplete(() => actionBefore.Invoke());
    }
}
