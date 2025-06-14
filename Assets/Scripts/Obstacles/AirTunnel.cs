using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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


    private void Awake()
    {

    }

    void Update()
    {
        if (!_isActive) {
            return;
        }

        _isActive = false;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.parent.TryGetComponent<Player>(out player))
        {
            _startPoint = player.transform.position;
            player.SetGlide(true);
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
        }
    }

    // Gizmo
    private void OnDrawGizmos()
    {
        Vector3? oldVec = null;
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
