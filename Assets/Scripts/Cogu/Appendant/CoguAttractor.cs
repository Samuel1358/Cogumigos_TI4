using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CoguAttractor : MonoBehaviour
{
    [Header("External Accesses")]
    [SerializeField] private CoguCastPoint _assingPoint;

    [Header("Settings")]
    [SerializeField] private float _maxExpand;
    [SerializeField] private float _expandSpd;
    [SerializeField] private float _colapseSpd;

    private Vector3 _expandVector = new Vector3(1, 0, 1);
    private Collider _collider;

    private void Start()
    {
        _collider = GetComponent<Collider>();
    }

    // INPUT - mudar depois
    private void Update()
    {
        if (Input.GetKey(KeyCode.F))
            Expand();
        else
            Colapse();
    }

    // Metodos Publicos
    public void Expand()
    {
        if (!_collider.enabled)
            _collider.enabled = true;

        if (transform.localScale.x < _maxExpand)
            transform.localScale += Time.deltaTime * _expandSpd * _expandVector;
    }

    public void Colapse()
    {
        if (!_collider.enabled) 
            return;

        if (transform.localScale.x > 0)
            transform.localScale -= Time.deltaTime * _colapseSpd * _expandVector;
        else if (transform.localScale.x <= 0)
        {
            transform.localScale = Vector3.up;
            _collider.enabled = false;
        }
    }

    // Collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out WildCogu wildCogu))
        {
            //Debug.Log(_assingPoint);
            wildCogu.Attract(_assingPoint);
        }
    }
}
