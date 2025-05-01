using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    // Fields
    [SerializeField] private float _rotationSpeed = 30f;

    // Properties
    public float rotationSpeed {  get { return _rotationSpeed; } set {  _rotationSpeed = value; } }

    void Update()
    {
        transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime);
    }
}
