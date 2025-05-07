using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private bool _active;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private Vector3 _direction;
    [SerializeField] private Space _space;

    private Vector3 _initialRotation;

    private void Update()
    {
        if (_active)
            transform.Rotate(_rotateSpeed * Time.deltaTime * _direction.normalized, _space);
    }

    // Get & Set
    public void SetActive(bool value)
    {
        _active = value;
    }

    // Public Methods
    public void ResetRotation()
    {
        transform.rotation = Quaternion.Euler(_initialRotation);
    }
}
