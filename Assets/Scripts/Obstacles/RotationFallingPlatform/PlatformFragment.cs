using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlatformFragment : CollisionHandlerChild
{
    [SerializeField] private float _fallDelay = 0.2f;
    [SerializeField] private float _restoreDelay = 2f;

    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private Rigidbody _rb;

    protected override void Start()
    {
        base.Start();

        _initialPosition = transform.localPosition;
        _initialRotation = transform.localRotation;
        _rb = GetComponent<Rigidbody>();

        _rb.isKinematic = true;
    }

    // Inherited public Methods
    public override void ParentCollisionEnter(Collision other)
    {
        if (other.gameObject.transform.position.y < transform.position.y - 0.2f)
            return;

        TweenHandler.FallingPlatformShake(transform, new Vector3(.5f, .5f, 1), _fallDelay, _restoreDelay, Fall, Restore);
    }

    private void Fall()
    {
        _rb.isKinematic = false;
    }

    private void Restore()
    {
        _rb.isKinematic = true;

        transform.localPosition = _initialPosition;
        transform.localRotation = _initialRotation;
    }
}
