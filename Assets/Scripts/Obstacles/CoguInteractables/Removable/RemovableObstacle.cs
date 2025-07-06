using UnityEngine;
using DG.Tweening;

public class RemovableObstacle : CoguInteractable
{
    [Header("Removable")]
    [SerializeField] private Transform _waypointsContainer;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _changeDistance;
    public bool startAvailable;

    private bool _walk = false;
    private int _index = 0;
    private bool _arrived = false;
    private Cogu _cogu;

    // memento
    private Vector3 _startPosition;
    private bool _availableAtCheckpoint;
    private bool _arrivedAtCheckpoint;
    //private Tween _actualTween = null;

    private void Awake()
    {
        _isAvailable = startAvailable;

        _startPosition = transform.position;
        _availableAtCheckpoint = _isAvailable;
        _arrivedAtCheckpoint = _arrived;
    }

    private void Update()
    {
        // NOJOO!!!
        if (!_walk)
            return;

        if (_index < _waypointsContainer.childCount - 1)
        {
            FollowPath();
        }
        else
        {
            Arrive();
        }
    }

    // Public Methods
    [ContextMenu("Walk")]
    public void StarWalk()
    {
        _walk = true;
    }

    // Private Methods
    private void FollowPath()
    {
        Transform point = _waypointsContainer.GetChild(_index);
        Vector3 dir = point.position - transform.position;

        float changeDistance = _changeDistance;
        float speedModifier = 1f;
        if (point.TryGetComponent(out RemovableWaypoint waypoint))
        {
            changeDistance = waypoint.ChangeDistanceModifier;
            speedModifier = waypoint.SpeedModifier;
        }

        if (dir.magnitude < changeDistance)
            _index++;

        transform.Translate(_moveSpeed * speedModifier * Time.fixedDeltaTime * dir.normalized);
    }

    private void Arrive()
    {
        Transform point = _waypointsContainer.GetChild(_index);
        Vector3 dir = point.position - transform.position;

        float changeDistance = _changeDistance;
        float speedModifier = 1f;
        if (point.TryGetComponent(out RemovableWaypoint waypoint))
        {
            changeDistance = waypoint.ChangeDistanceModifier;
            speedModifier = waypoint.SpeedModifier;
        }

        if (dir.magnitude < changeDistance)
        {
            _walk = false;
            Destroy(_cogu.gameObject);
        }

        transform.Translate(_moveSpeed * speedModifier * Time.fixedDeltaTime * dir.normalized);
    }

    // Interactable
    public override void Interact(Cogu cogu)
    {
        _cogu = cogu;
        StarWalk();
        NeedReset = true;
    }

    // Resetable
    public override void ResetObject()
    {
        if (NeedReset)
        {
            base.ResetObject();

            _walk = false;
            _index = 0;

            transform.position = _startPosition;
            _isAvailable = _availableAtCheckpoint;
            _arrived = _arrivedAtCheckpoint;

            NeedReset = false;
        }
    }

    private void OnDrawGizmos()
    {
        //if (Application.isPlaying)
        //    return;

        if (_waypointsContainer == null)
            return;

        Vector3 from = transform.position;

        Gizmos.color = Color.yellow;
        for (int i = 0; i < _waypointsContainer.childCount; i++)
        {
            Vector3 point = _waypointsContainer.GetChild(i).position;
            Gizmos.DrawLine(from, point);           
            from = point;

            if (i == _waypointsContainer.childCount - 1)
                Gizmos.color = Color.red;
            else
                Gizmos.color = Color.blue;

            Gizmos.DrawSphere(point, 0.05f);

            Gizmos.color = Color.white;
        }

        Gizmos.color = Color.white;
    }
}
