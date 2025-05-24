using UnityEngine;

public class RotatingPlatformSticky : ParentagePlatform
{
    // Fields
    [SerializeField] private float _rotationSpeed = 30f;

    // Properties
    public float rotationSpeed {  get { return _rotationSpeed; } set {  _rotationSpeed = value; } }

    // Inherited Public Properties
    public override bool lockColliderMode { get { return true; } }

    private void Awake()
    {
        _collideMode = CollideMode.BothColision;
    }

    void Update()
    {
        transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime);
    }
}
