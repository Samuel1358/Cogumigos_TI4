using System.Collections.Generic;
using UnityEngine;

public class AirTunnel : MonoBehaviour {
    [SerializeField] private List<Transform> _pathPoints;
    [Header("Speed Based")]
    [SerializeField] private float _speed = 5f;
    [Header("Time Based")]
    [SerializeField] private bool _moveByTime = false;
    [SerializeField] private float _totalTime = 10f;

    private Transform _objToMove;
    private bool _isActive;
    private Player player;

    private int _currentIndex = 0;
    private float _journeyLength = 0f;
    private float _calculatedSpeed;

    private Vector3 _startPoint;
    private Vector3 _targetPoint;
    private float _segmentDistance;
    private float _segmentDuration;
    private float _segmentProgress;


    private void Awake() {
        if (_moveByTime) {
            _journeyLength = 0f;
            for (int i = 0; i < _pathPoints.Count - 1; i++) {
                _journeyLength += Vector3.Distance(_pathPoints[i].position, _pathPoints[i + 1].position);
            }

            _calculatedSpeed = _journeyLength / _totalTime;
        }
        SetupNextSegment();
    }

    void Update() {
        if (_isActive) {
            _segmentProgress += Time.deltaTime;

            float t = _segmentProgress / _segmentDuration;
            _objToMove.position = Vector3.Slerp(_startPoint, _targetPoint, t);

            if (_segmentProgress >= _segmentDuration) {
                _objToMove.position = _targetPoint;
                SetupNextSegment();
            }
        }
    }

    // Private Methods
    private void ResetTunnel()
    {
        _isActive = false;
        player.SetGlide(false);
        _objToMove = null;
        _currentIndex = 0;
        _segmentProgress = 0f;
    }

    private void SetupNextSegment() 
    {
        if (_currentIndex >= _pathPoints.Count - 1) {
            _currentIndex = _pathPoints.Count;
            ResetTunnel();
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

    private void OnTriggerEnter(Collider collider) {
        if (collider.transform.parent.TryGetComponent<Player>(out player)) {
            Debug.Log("Colided player");
            _objToMove = player.transform;
            _startPoint = player.transform.position;
            player.SetGlide(true);
            _currentIndex = 0;
            for (int i = 0; i < _pathPoints.Count - 1; i++) {
                if (Vector3.Distance(_pathPoints[i].position, _startPoint) < Vector3.Distance(_pathPoints[_currentIndex].position, _startPoint)) {
                    _currentIndex = i;
                }
            }
            _targetPoint = _pathPoints[_currentIndex].position;
            _isActive = true;
        }
    }

    // Gizmo
    private void OnDrawGizmosSelected()
    {
        Vector3? oldVec = null;
        foreach (var pathPoint in _pathPoints)
        {
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
