using System;
using UnityEngine;
public class RemovableObstacle : CoguInteractable
{
    [SerializeField] private bool _startAvailable = true;
    [SerializeField] public Vector3 destiny = Vector3.forward;
    [SerializeField] public bool positionated;

    private Vector3 _inictialPosition;

    private void Awake()
    {
        _isAvailable = _startAvailable;
        destiny = transform.TransformPoint(destiny);

        _inictialPosition = transform.position;
    }

    [ContextMenu("Walk")]
    public void Walk()
    {
        if (positionated)
        {
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
            _isAvailable = true;
            NeedReset = false;
        }
    }

    // Gizmo
    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        if (Application.isPlaying)
            return;

        Color oldGizmoColor = Gizmos.color;

        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.TransformPoint(destiny));

        Gizmos.color = oldGizmoColor;
    }
}
