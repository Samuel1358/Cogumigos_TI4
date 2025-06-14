using UnityEngine;
using DG.Tweening;
public class Dissolve : MonoBehaviour {
    [SerializeField] private float _duration;
    async private void Start() {
        Sequence sequence = DOTween.Sequence();
        sequence.Join(transform.DOShakeScale(_duration/2.0f).SetEase(Ease.InOutElastic));
        sequence.Join(transform.DOScale(0.01f, _duration/2.0f).SetEase(Ease.InBounce));
        await sequence.AsyncWaitForCompletion();
        Destroy(gameObject);
    }
}
