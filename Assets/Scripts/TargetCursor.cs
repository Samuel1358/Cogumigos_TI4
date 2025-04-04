using UnityEngine;

public class TargetCursor : MonoBehaviour
{
    [Header("External Access")]
    [SerializeField] private Transform playerTrasnform;
    [SerializeField] private CoguAttractor coguAttractor;

    [Header("Settings")]
    [SerializeField] private LayerMask includeLayers;
    [SerializeField] private float maxDiatance;
    [SerializeField] private Vector3 offset;

    private Vector3 _hitPoint;

    // Metodos Publicos
    public void UpdatePosition(Vector3 mousePositionInput)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(mousePositionInput);

        if (Physics.Raycast(ray, out hit, 100f, includeLayers))
        {
            Vector3 point = hit.point;

            if (Vector3.Distance(point, playerTrasnform.position) > maxDiatance)
            {
                point = playerTrasnform.position + ((hit.point - playerTrasnform.position).normalized * maxDiatance);

                if (Physics.Raycast(Camera.main.ScreenPointToRay(point), out hit, 100f, includeLayers))
                {
                    PositionCursorTarget(point, hit);
                }
            }

            PositionCursorTarget(point, hit);
        }
    }

    public void AttractFriendshroom(bool input)
    {
        if (input)
        {
            if (coguAttractor.gameObject.activeSelf == false)
            {
                coguAttractor.gameObject.SetActive(true);
            }
            coguAttractor.Expand();
        }
        else
        {
            if (coguAttractor.gameObject.activeSelf == true)
            {
                coguAttractor.Colapse();
            }
        }
    }

    // Metodos Privados
    private void PositionCursorTarget(Vector3 pos, RaycastHit hit)
    {
        _hitPoint = pos;
        transform.position = _hitPoint + offset;
        transform.up = Vector3.Lerp(transform.up, hit.normal, 0.3f);
    }
}
