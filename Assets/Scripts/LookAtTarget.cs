using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [SerializeField] private Transform _target;

    public void LookAt()
    {
        if (_target == null)
            return;

        //Vector3 lookDirection = transform.position + _target.forward;
        transform.LookAt(_target);
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
}
