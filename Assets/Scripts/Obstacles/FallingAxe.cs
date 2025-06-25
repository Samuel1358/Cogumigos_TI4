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
    [SerializeField] private float _startCooldown;

    [Header("Infinite Settings")]
    
    [SerializeField] private bool infiniteRotation = false;
    [SerializeField] private Vector3 _axesRotation;
    [SerializeField] private float _infiniteRotationTime = 2f;

    async private void Start() {
        await Task.Delay((int)(_startCooldown * 1000));
        Sequence sequence = DOTween.Sequence();
        Vector3 rotationDirection = new(0f, 0f, targetRotation);
        Vector3 originalRotation = transform.rotation.eulerAngles;
        rotationDirection = originalRotation + rotationDirection;
        if (infiniteRotation) {
            Vector3 rotateAxe = new Vector3(360 * _axesRotation.x, 360 * _axesRotation.y, 360 * _axesRotation.z);
            transform.DORotate(rotateAxe, _infiniteRotationTime, RotateMode.FastBeyond360).SetRelative(true)
           .SetEase(Ease.Linear).SetLoops(-1);
        }
        else {
            sequence.Append(transform.DORotate(rotationDirection, fallDuration).SetEase(Ease.Linear))
                .Append(transform.DOShakePosition(groundShakeDuration))
                .Append(transform.DORotate(originalRotation, upDuration).SetEase(Ease.Linear).SetDelay(groundStaticDuration))
                .Append(transform.DOShakePosition(airShakeDuration / 3).SetDelay((airShakeDuration * 2 / 3))).SetLoops(-1);
        }
    }


}