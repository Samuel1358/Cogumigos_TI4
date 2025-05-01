using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlatformFragment : MovingPlatform
{
    [SerializeField] private float _fallDelay = 0.2f;
    [SerializeField] private float _restoreDelay = 2f;

    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private Rigidbody _rb;

    void Start()
    {
        _initialPosition = transform.localPosition;
        _initialRotation = transform.localRotation;
        _rb = GetComponent<Rigidbody>();

        _rb.isKinematic = true;
    }

    [ContextMenu("Fall")]
    public void Fall()
    {
        StartCoroutine(FallAndRestore());
    }

    protected override void CollisionEnter(Collision collision)
    {
        StartCoroutine(FallAndRestore());
    }

    protected override void TriggerEnter(Collider other)
    {
        StartCoroutine(FallAndRestore());
    }

    IEnumerator FallAndRestore()
    {
        // prestes a cair

        yield return new WaitForSeconds(_fallDelay);

        // caindo
        _rb.isKinematic = false;

        yield return new WaitForSeconds(_restoreDelay);

        // respawn
#pragma warning disable CS0618 // O tipo ou membro é obsoleto
        _rb.velocity = Vector3.zero;
#pragma warning restore CS0618 // O tipo ou membro é obsoleto
        _rb.angularVelocity = Vector3.zero;
        _rb.isKinematic = true;

        transform.localPosition = _initialPosition;
        transform.localRotation = _initialRotation;
    }
}
