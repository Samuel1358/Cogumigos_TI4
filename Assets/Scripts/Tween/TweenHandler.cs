using UnityEngine;
using DG.Tweening;
using System;

public static class TweenHandler
{
    public static float shakeStrenght = 2f;
    public static int shakeVibrato = 5;
    public static float randomness = 30f;   

    // Private Methods
    private static Tweener Timer(float time, Action actionBefore)
    {
        float count = 0;
        return DOTween.To(() => count, x => count = x, 10, time).OnComplete(() => actionBefore.Invoke());
    }

    private static Tweener ShakeRotation(Transform transform, Vector3 shakeDirection, float time, Action actionBefore)
    {
        return DOTween.Shake(() => transform.localRotation.eulerAngles,
            x => transform.localRotation = Quaternion.Euler(x),
            time, shakeDirection.normalized * shakeStrenght,
            shakeVibrato, randomness).OnComplete(() => actionBefore.Invoke());
    }

    // Public Methods
    public static void FallingPlatformShake(Transform transform, Vector3 shakeDirection, float fallTime, float restoreTime, Action fallAction, Action restoreAction)
    {
        Tweener fall = ShakeRotation(transform, shakeDirection, fallTime, fallAction);
        Tweener restore = Timer(restoreTime, restoreAction);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(fall);
        sequence.Append(restore);
    }

    public static void MoveX(Transform transform, float moveValue, float duration, Ease ease)
    {
        transform.DOMoveX(moveValue, duration).SetEase(ease);
    }

    public static void MoveY(Transform transform, float moveValue, float duration, Ease ease)
    {
        transform.DOMoveY(moveValue, duration).SetEase(ease);
    }

    public static void MoveZ(Transform transform, float moveValue, float duration, Ease ease)
    {
        transform.DOMoveZ(moveValue, duration).SetEase(ease);
    }

    public static void Rotate(Transform transform, Vector3 endRotate, float duration, Ease ease)
    {
        transform.DORotate(endRotate, duration).SetEase(ease);
    }
}
