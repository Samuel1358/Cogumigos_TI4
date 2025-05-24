using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private bool _active;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private Vector3 _direction;
    [SerializeField] private Space _space;

#pragma warning disable CS0649 // Campo "Rotate._initialRotation" nunca é atribuído e sempre terá seu valor padrão 
    private Vector3 _initialRotation;
#pragma warning restore CS0649 // Campo "Rotate._initialRotation" nunca é atribuído e sempre terá seu valor padrão 

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
