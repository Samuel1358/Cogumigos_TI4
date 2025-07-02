using UnityEngine;
using DG.Tweening;

public class RemovableObstacle : CoguInteractable
{
    [Header("Removable")]
    [SerializeField] private Transform _waypointsContainer;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _changeDistance;

    private int _index = 0;
    private bool _arrived = false;

    // Public Methods
    [ContextMenu("Walk")]
    public void StarWalk(Cogu cogu)
    {
        Tween follow = DOTween.To(IndexGetter, FollowPath, _waypointsContainer.childCount - 1, float.MaxValue);
        follow.OnUpdate(() => Arrive(follow, cogu));
    }

    // Private Methods
    private int IndexGetter()
    {
        return _index;
    }

    private void FollowPath(int x)
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

        _arrived = (_index >= _waypointsContainer.childCount);

        transform.Translate(_moveSpeed * speedModifier * Time.fixedDeltaTime * dir.normalized);
    }

    private void Arrive(Tween follow, Cogu cogu)
    {
        if (_arrived)
        {
            follow.Kill();

            Transform endPoint = _waypointsContainer.GetChild(_index - 1);
            float dis = Mathf.Abs(Vector3.Distance(endPoint.position, transform.position));
            Tween end;
            if (endPoint.TryGetComponent(out RemovableWaypoint waypoint))
                end = transform.DOMove(endPoint.position, dis / (_moveSpeed * waypoint.SpeedModifier));
            else
                end = transform.DOMove(endPoint.position, dis / _moveSpeed);

            end.OnComplete(() => Destroy(cogu.gameObject));
        }
    }

    // Interactable
    public override void Interact(Cogu cogu)
    {
        Debug.Log("RemovableObject - Interact");
        StarWalk(cogu);
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
