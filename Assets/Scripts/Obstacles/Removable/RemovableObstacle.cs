using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class RemovableObstacle : MonoBehaviour, IInteractable
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] public Vector3 destiny = Vector3.forward;
    [SerializeField] public bool positionated;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        destiny = transform.TransformPoint(destiny);
    }

    [ContextMenu("Walk")]
    public void Walk()
    {
        if (positionated)
        {
            _agent.SetDestination(destiny);
        }
    }

    public Action Interact(Cogu cogu)
    {
        return () => { };
    }

    // Gizmo
    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
            return;

        Color oldGizmoColor = Gizmos.color;

        Gizmos.color = Color.white;
        Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y - (transform.localScale.y / 2), transform.position.z), transform.TransformPoint(destiny));

        Gizmos.color = oldGizmoColor;
    }
}
