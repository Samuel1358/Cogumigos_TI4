using DG.Tweening;
using UnityEngine;

public class LeverDoor : Switchable {
    [SerializeField] private float _doorMovement;
    [SerializeField] private float _doorMovementDuration;
    private Vector3 Origin;
    private void Awake() {
        Origin = transform.position;
    }
    public override void Activate() {
        transform.DOMoveY(_doorMovement, _doorMovementDuration);
    }

    public override void Disable() {
        transform.DOMoveY(-_doorMovement, _doorMovementDuration);
    }

    public override void ResetObject() {
        //transform.position = Origin;
    }
}
