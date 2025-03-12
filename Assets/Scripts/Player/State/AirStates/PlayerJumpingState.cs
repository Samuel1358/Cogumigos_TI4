using System;
using UnityEngine;

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

        Jump();
        StateMachineMovement.ReusableData.MovementSpeedModifier = _jumpData.SpeedModifier; ;
        StateMachineMovement.ReusableData.MovementDecelerationForce = _jumpData.DecelerationForce;
        _shouldKeepRotating = StateMachineMovement.ReusableData.MovementInput != Vector2.zero;
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
        if(!_canStartFalling || GetPlayerVerticalVelocity().y > 0) {
            return;
        }
        StateMachineMovement.ChangeState(StateMachineMovement.FallingState);
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
        if (_shouldKeepRotating) {
            UpdateTargetRotation(GetInputDirection());
            jumpDirection = GetTargetRotationDirection(StateMachineMovement.ReusableData.CurrentTargetRotation.y);
        }
        jumpForce.x *= jumpDirection.x;
        jumpForce.z *= jumpDirection.z;

        Vector3 capsuleColiderCenterInWorldSpace = StateMachineMovement.PlayerGet.ColliderUtility.CapsuleColliderData.Collider.bounds.center;
        Ray downardsRayFromCapsuleCenter = new Ray(capsuleColiderCenterInWorldSpace, Vector3.down);
        if (Physics.Raycast(downardsRayFromCapsuleCenter, out RaycastHit hit, _jumpData.JumpToGroundRayDistance, StateMachineMovement.PlayerGet.LayerData.GroundLayerMask, QueryTriggerInteraction.Ignore)) {
            float groundAngle = Vector3.Angle(hit.normal, -downardsRayFromCapsuleCenter.direction);

            if (IsMovingUp()) {
                float forceModifier = _jumpData.JumpForceModfierOnSlopeUpwards.Evaluate(groundAngle);
                jumpForce.x *= forceModifier;
                jumpForce.z *= forceModifier;
            }else if (IsMovingDown()) {
                float forceModifier = _jumpData.JumpForceModfierOnSlopeDownwards.Evaluate(groundAngle);
                jumpForce.y *= forceModifier;
            }
        }

        ResetVelocity();
        StateMachineMovement.PlayerGet.PlayerRigidbody.AddForce(jumpForce, ForceMode.VelocityChange);
        Debug.Log(StateMachineMovement.PlayerGet.PlayerRigidbody.linearVelocity);
    }
    protected override void DoubleJump() {
        Jump();
    }

    protected override void ResetSpringState() {
    }
}
