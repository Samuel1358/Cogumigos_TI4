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
        StartCoroutine(FallAndRestore());
    }

    // Private Methods
    private IEnumerator FallAndRestore()
    {
        // prestes a cair
        AudioManager.Instance.PlaySFX("PlatformBreak");
        yield return new WaitForSeconds(_fallDelay);

        // caindo
        _rb.isKinematic = false;
        AudioManager.Instance.StopSFX();
        yield return new WaitForSeconds(_restoreDelay);

        // respawn
#pragma warning disable CS0618 // O tipo ou membro � obsoleto
        _rb.velocity = Vector3.zero;
#pragma warning restore CS0618 // O tipo ou membro � obsoleto
        _rb.angularVelocity = Vector3.zero;
        _rb.isKinematic = true;

        transform.localPosition = _initialPosition;
        transform.localRotation = _initialRotation;
    }
}
