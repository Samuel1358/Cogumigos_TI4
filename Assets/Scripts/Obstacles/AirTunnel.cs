using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using DG.Tweening;

public class AirTunnel : MonoBehaviour
{

    [SerializeField] private Transform _waypointsContainer;
    [SerializeField] private float _moveSpeed;
    //[SerializeField] private float _changeDistance;
    private Player _target;

    private int _index = 0;

    public Transform WaypointsContainer { get { return _waypointsContainer; } }

    public void StartGlide(Player target)
    {
        _target = target;

        Transform point = _waypointsContainer.GetChild(_index);
        float dis = Mathf.Abs(Vector3.Distance(point.position, _target.transform.position));
        float t = dis / _moveSpeed;

        _target.transform.DOMove(point.position, t)
            .SetEase(Ease.Linear)
            .OnComplete(NextWaypoint);
    }

    // Private Methods
    private bool Validate()
    {
        if (_waypointsContainer == null)
            return false;

        if (_waypointsContainer.childCount < 2)
            return false;

        if (_target == null)
            return false;

        return true;
    }

    private void NextWaypoint()
    {
        if (!Validate())
        {
            Arrive();
        }

        _index++;
        if (_index >= _waypointsContainer.childCount)
        {
            Arrive();
            return;
        }

        Transform point = _waypointsContainer.GetChild(_index);
        float dis = Mathf.Abs(Vector3.Distance(point.position, _target.transform.position));
        float t = dis / _moveSpeed;

        _target.transform.DOMove(point.position, t)
            .SetEase(Ease.Linear)
            .OnComplete(NextWaypoint);
    }

    private void Arrive()
    {
        _target.SetGlide(false);
        _target = null;
        _index = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();
        if (player == null)
            return;

        StartGlide(player);
        player.SetGlide(true);
    }

    private void OnDrawGizmos()
    {
        //if (Application.isPlaying)
        //    return;

        if (_waypointsContainer == null)
            return;

        if (_waypointsContainer.childCount < 2)
            return;

        Vector3 from = _waypointsContainer.GetChild(0).position;

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(from, 0.05f);

        Gizmos.color = Color.white;
        for (int i = 1; i < _waypointsContainer.childCount; i++)
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

#if UNITY_EDITOR
public class AirTunnelEditor : Editor
{
    private AirTunnel _airTunnel;

    private void OnEnable()
    {
        _airTunnel = target as AirTunnel;
    }

    public override void OnInspectorGUI()
    {
        if (_airTunnel.WaypointsContainer == null)
            EditorGUILayout.HelpBox("WaypointContainer has to be assigned", MessageType.Error);
        else if (_airTunnel.WaypointsContainer.childCount < 2)
            EditorGUILayout.HelpBox("the container must have at least 2 waypoints", MessageType.Error);

        base.OnInspectorGUI();
    }
}
#endif