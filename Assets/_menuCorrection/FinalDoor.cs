using DG.Tweening;
using UnityEngine;

public class FinalDoor : MonoBehaviour
{
    [SerializeField] private float _doorMovement;
    [SerializeField] private float _doorMovementDuration;
    private Vector3 Origin;
    private void Awake() {
        Origin = transform.position;
    }
    public void Activate() {
        transform.DOMoveY(_doorMovement, _doorMovementDuration);
    }
}
