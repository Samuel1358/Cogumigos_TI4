using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;


public class RandolyPathFeedback : MonoBehaviour
{
    [SerializeField] private Transform _platformContainer;
    [SerializeField] private Transform _fakePlatformContainer;
    [SerializeField] private float _heightDislocation;
    [SerializeField] private float _duration;
    [SerializeField] private float _waitTime;
    [SerializeField] private Ease _easeMode;

    [Header("Lillypad Override Settings")]
    [SerializeField] private bool _overrideLillypad = false;
    [Space]
    [SerializeField] public float xAmplitude = 0.5f;
    [SerializeField] public float zAmplitude = 0.3f;
    [SerializeField] public float xSpeed = 1.0f;
    [SerializeField] public float zSpeed = 0.8f;
    [SerializeField] public float rotationAmplitude = 2.0f;
    [SerializeField] public float rotationSpeed = 0.5f;

    private List<RandolyPathPlatform> _platformList = new List<RandolyPathPlatform>();
    private List<RandolyPathFakePlatform> _fakePlatformList = new List<RandolyPathFakePlatform>();

    // Public Methods
    public void AddPlatform(GameObject obj)
    {
        if (obj.TryGetComponent(out RandolyPathPlatform platform))
        {
            _platformList.Add(platform);
            obj.transform.parent = _platformContainer;

            if (_overrideLillypad)
                platform.OverrideLillypadSettings(xAmplitude, zAmplitude, xSpeed, zSpeed, rotationAmplitude, rotationSpeed);
        }
        else if (obj.TryGetComponent(out RandolyPathFakePlatform fakePlatform))
        {
            _fakePlatformList.Add(fakePlatform);
            obj.transform.parent = _fakePlatformContainer;

            if (_overrideLillypad)
                fakePlatform.OverrideLillypadSettings(xAmplitude, zAmplitude, xSpeed, zSpeed, rotationAmplitude, rotationSpeed);
        }
        else
            Destroy(obj);
    }

    public void StartFeedback()
    {        
        Tween platformsDeslocation = _platformContainer.DOMoveY(transform.position.y + _heightDislocation, _duration)
            .SetEase(_easeMode);
        Tween fakePlatformsDeslocation = _fakePlatformContainer.DOMoveY(transform.position.y + _heightDislocation, _duration)
            .SetEase(_easeMode);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(platformsDeslocation);
        sequence.Append(TweenHandler.Timer(_waitTime));
        sequence.Append(fakePlatformsDeslocation);

        sequence.OnComplete(StartPuzzle);
    }

    // Private Methods
    private void StartPuzzle()
    {
        foreach (var platform in _platformList)
        {
            platform.SetEnabledLillypad(true);
        }
        foreach (var fakePlatform in _fakePlatformList)
        {
            fakePlatform.SetEnabledLillypad(true);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 endPoint = transform.position + (Vector3.up * _heightDislocation);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, endPoint);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(endPoint, 0.15f);
    }
}
