using UnityEngine;

public class TargetCursor : MonoBehaviour
{
    [Header("External Access")]
    [SerializeField] private Transform _playerTrasnform;
    [SerializeField] private CoguAttractor _coguAttractor;
    [SerializeField] private CoguCastter _coguCastter;

    [Header("Settings")]
    [SerializeField] private LayerMask _includeLayers;
    [SerializeField] private float _maxDiatance;
    [SerializeField] private Vector3 _offset;

    [Header("Send Cogu")]
    [SerializeField] private float _interactRadius;
    [SerializeField] private LayerMask _interactIncludeLayers;

    private Vector3 _hitPoint;

    // INPUT - mudar depois
    private void Update()
    {
        UpdatePosition(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            SendCogu();
        }
    }

    // Metodos Publicos
    public void UpdatePosition(Vector3 mousePositionInput)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(mousePositionInput);

        if (Physics.Raycast(ray, out hit, 100f, _includeLayers))
        {
            Vector3 point = hit.point;

            if (Vector3.Distance(point, _playerTrasnform.position) > _maxDiatance)
            {
                point = _playerTrasnform.position + ((hit.point - _playerTrasnform.position).normalized * _maxDiatance);

                if (Physics.Raycast(Camera.main.ScreenPointToRay(point), out hit, 100f, _includeLayers))
                {
                    PositionCursorTarget(point, hit);
                }
            }

            PositionCursorTarget(point, hit);
        }
    }

    public void SendCogu()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _interactRadius, _interactIncludeLayers, QueryTriggerInteraction.Collide);
        foreach (Collider obj in colliders)
        {
            if (obj.TryGetComponent(out CoguInteractable interactable))
            {
                if (interactable.IsAvailable)
                {
                    _coguCastter.CastCogu(interactable.AssignedCoguName, transform.position, interactable);
                    return;
                }               
            }
        }
    }

    // Metodos Privados
    private void PositionCursorTarget(Vector3 pos, RaycastHit hit)
    {
        _hitPoint = pos;
        transform.position = _hitPoint + _offset;
        transform.up = Vector3.Lerp(transform.up, hit.normal, 0.3f);
    }

}
