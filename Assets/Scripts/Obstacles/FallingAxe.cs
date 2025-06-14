using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
public class FallingAxe : MonoBehaviour {
    [Header("Movement Settings")]
    [SerializeField] private float targetRotation = 90f;
    [SerializeField] private float upDuration = 1f;
    [SerializeField] private float fallDuration = 1f;
    [SerializeField] private float groundShakeDuration = 2f;
    [SerializeField] private float airShakeDuration = 3f;
    [SerializeField] private float groundStaticDuration;
    [SerializeField] private bool infiniteRotation = false;

    [Header("Start Settings")]
    [SerializeField] private float _startCooldown;

    async private void Start() {
        await Task.Delay((int)(_startCooldown * 1000));
        Sequence sequence = DOTween.Sequence();
        Vector3 rotationDirection = new(0f, 0f, targetRotation);
        Vector3 originalRotation = transform.rotation.eulerAngles;
        rotationDirection = originalRotation + rotationDirection;
        sequence.Append(transform.DORotate(rotationDirection, fallDuration).SetEase(Ease.Linear))
            .Append(transform.DOShakePosition(groundShakeDuration))
            .Append(transform.DORotate(originalRotation, upDuration).SetEase(Ease.Linear).SetDelay(groundStaticDuration))
            .Append(transform.DOShakePosition(airShakeDuration/3).SetDelay((airShakeDuration * 2 / 3))).SetLoops(-1);
    }


}
