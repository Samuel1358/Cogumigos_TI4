using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class RemovableObstacle : CoguInteractable
{
    private NavMeshAgent _agent;
    [SerializeField] private bool _startAvailable = true;
    [SerializeField] public Vector3 destiny = Vector3.forward;
    [SerializeField] public bool positionated;

    private Vector3 _inictialPosition;

    private void Awake()
    {
        _isAvailable = _startAvailable;

        _agent = GetComponent<NavMeshAgent>();
        destiny = transform.TransformPoint(destiny);

        _inictialPosition = transform.position;
    }

    public void Walk()
    {
        if (positionated)
        {
            _agent.SetDestination(destiny);
            NeedReset = true;
        }
    }

    public override Action Interact(Cogu cogu)
    {
        Walk();
        return () => { Destroy(cogu.gameObject); };
    }

    // Resetable
    public override void ResetObject()
    {
        if (NeedReset)
        {
            transform.position = _inictialPosition;
            _agent.SetDestination(transform.position);
            _isAvailable = true;
            NeedReset = false;
        }
    }

    // Gizmo
    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
            return;

        Color oldGizmoColor = Gizmos.color;

        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.TransformPoint(destiny));

        Gizmos.color = oldGizmoColor;
    }
}
