using System.Collections.Generic;
using UnityEngine;

public class RollingStone : MonoBehaviour
{
    [SerializeField] private List<Transform> _pathPoints;
    [SerializeField] private Transform _objToMove;
    [Header("Speed Based")]
    [SerializeField] private float _speed = 5f;
    [Header("Time Based")]
    [SerializeField] private bool _moveByTime = false;
    [SerializeField] private float _totalTime = 10f;

    private bool _isActive;

    private int _currentIndex = 0;
    private float _journeyLength = 0f;
    private float _calculatedSpeed;
    private bool _once = true;

    private Vector3 _startPoint;
    private Vector3 _targetPoint;
    private float _segmentDistance;
    private float _segmentDuration;
    private float _segmentProgress;


    private void Awake()
    {
        if (_moveByTime)
        {
            _journeyLength = 0f;
            for (int i = 0; i < _pathPoints.Count - 1; i++)
            {
                _journeyLength += Vector3.Distance(_pathPoints[i].position, _pathPoints[i + 1].position);
            }

            _calculatedSpeed = _journeyLength / _totalTime;
        }
        SetupNextSegment();

        _startPoint = _objToMove.position;
    }

    void Update()
    {
        if (_isActive)
        {
            _segmentProgress += Time.deltaTime;

            float t = _segmentProgress / _segmentDuration;
            _objToMove.position = Vector3.Slerp(_startPoint, _targetPoint, t);

            if (_segmentProgress >= _segmentDuration)
            {
                _objToMove.position = _targetPoint;
                SetupNextSegment();
            }
        }
    }

    private void SetupNextSegment()
    {
        if (_currentIndex >= _pathPoints.Count - 1)
        {
            _isActive = false;
            return;
        }

        _startPoint = _pathPoints[_currentIndex].position;
        _targetPoint = _pathPoints[_currentIndex + 1].position;
        _segmentDistance = Vector3.Distance(_startPoint, _targetPoint);

        float currentSpeed = _moveByTime ? _calculatedSpeed : _speed;
        _segmentDuration = _segmentDistance / currentSpeed;
        _segmentProgress = 0f;

        _currentIndex++;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (_once)
            if (collider.transform.parent.TryGetComponent(out Player player))
            {
                _currentIndex = 0;
                for (int i = 0; i < _pathPoints.Count - 1; i++)
                {
                    if (Vector3.Distance(_pathPoints[i].position, _startPoint) < Vector3.Distance(_pathPoints[_currentIndex].position, _startPoint))
                    {
                        _currentIndex = i;
                    }
                }
                _targetPoint = _pathPoints[_currentIndex].position;
                _isActive = true;
                _once = false;
            }
    }

    // TESTE
    [ContextMenu("Teste")]
    public void Teste()
    {
        if (_once)
        {
            _currentIndex = 0;
            for (int i = 0; i < _pathPoints.Count - 1; i++)
            {
                if (Vector3.Distance(_pathPoints[i].position, _startPoint) < Vector3.Distance(_pathPoints[_currentIndex].position, _startPoint))
                {
                    _currentIndex = i;
                }
            }
            _targetPoint = _pathPoints[_currentIndex].position;
            _isActive = true;
            _once = false;
        }
        else
        {
            Debug.LogWarning("Não pode!");
        }
    }

    // Gizmo
    private void OnDrawGizmos()
    {
        Vector3? oldVec = null;
        if (_objToMove != null)
            oldVec = _objToMove.position;

        foreach (var pathPoint in _pathPoints)
        {
            if (pathPoint != null)
                if (oldVec == null)
                {
                    oldVec = pathPoint.position;
                }
                else
                {
                    Gizmos.DrawLine(oldVec.Value, pathPoint.position);
                    oldVec = pathPoint.position;
                }
        }
    }
}
