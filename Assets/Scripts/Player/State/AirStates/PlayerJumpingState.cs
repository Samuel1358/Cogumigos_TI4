using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJumpingState : PlayerAirState {
    private bool _shouldKeepRotating;
    private bool _canStartFalling;
    private PlayerJumpData _jumpData;
    public PlayerJumpingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
        _jumpData = AirData.JumpData;
        StateMachineMovement.ReusableData.RotationData = _jumpData.RotationData;
    }

    public override void Enter() {
        base.Enter();

        StateMachineMovement.ReusableData.MovementSpeedModifier = _jumpData.SpeedModifier; ;
        StateMachineMovement.ReusableData.MovementDecelerationForce = _jumpData.DecelerationForce;
        StateMachineMovement.ReusableData.RotationData = _jumpData.RotationData;
        _shouldKeepRotating = StateMachineMovement.ReusableData.MovementInput != Vector2.zero;

        Jump();
    }
    public override void Exit() {
        base.Exit();

        SetBaseRotationData();

        _canStartFalling = false;
    }

    public override void Update() {
        base.Update();
        if (!_canStartFalling && IsMovingUp(0f)) {
            _canStartFalling = true;
        }
        if(!_canStartFalling || IsMovingUp(0f)) {
            return;
        }
        //StateMachineMovement.ChangeState(StateMachineMovement.FallingState);
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
        if (_shouldKeepRotating) {
            RotateTowardsTargetRotation();
        }
        if (IsMovingUp()) {
            DeceleationVertically();
        }
    }

    private void Jump() {
        Vector3 jumpForce = StateMachineMovement.ReusableData.CurrentJumpforce;

        Vector3 jumpDirection = StateMachineMovement.PlayerGet.transform.forward;

        jumpForce.x *= jumpDirection.x;
        jumpForce.z *= jumpDirection.z;

        jumpForce = GetJumpForceOnSlope(jumpForce);

        ResetVelocity();

        StateMachineMovement.PlayerGet.PlayerRigidbody.AddForce(jumpForce, ForceMode.VelocityChange);
    }

    private Vector3 GetJumpForceOnSlope(Vector3 jumpForce) {
        Vector3 capsuleColliderCenterInWorldSpace = StateMachineMovement.PlayerGet.ColliderUtility.CapsuleColliderData.Collider.bounds.center;

        Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

        if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, _jumpData.JumpToGroundRayDistance, StateMachineMovement.PlayerGet.LayerData.GroundLayerMask, QueryTriggerInteraction.Ignore)) {
            float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);

            if (IsMovingUp()) {
                float forceModifier = _jumpData.JumpForceModfierOnSlopeUpwards.Evaluate(groundAngle);

                jumpForce.x *= forceModifier;
                jumpForce.z *= forceModifier;
            }

            if (IsMovingDown()) {
                float forceModifier = _jumpData.JumpForceModfierOnSlopeDownwards.Evaluate(groundAngle);

                jumpForce.y *= forceModifier;
            }
        }

        return jumpForce;
    }

    protected override void DoubleJump() {
        Jump();
    }

    protected override void ResetSpringState() {
    }
}
