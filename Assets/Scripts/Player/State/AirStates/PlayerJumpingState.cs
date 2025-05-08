using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJumpingState : PlayerAirState {
    private bool _shouldKeepRotating;
    public PlayerJumpingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
        StateMachineMovement.ReusableData.RotationData = AirData.JumpData.RotationData;
    }

    public override void Enter() {
        base.Enter();

        StateMachineMovement.ReusableData.MovementSpeedModifier = AirData.JumpData.SpeedModifier;
        StateMachineMovement.ReusableData.RotationData = AirData.JumpData.RotationData;
        _shouldKeepRotating = StateMachineMovement.ReusableData.MovementInput != Vector2.zero;
        AudioManager.Instance.PlaySFX("PlayerJump");
        Jump();
    }

    private void Jump() {
        Vector3 jumpForce = new Vector3(StateMachineMovement.PlayerGet.PlayerRigidbody.linearVelocity.x, InitialJumpVelocity, StateMachineMovement.PlayerGet.PlayerRigidbody.linearVelocity.z);
        jumpForce = GetJumpForceOnSlope(jumpForce);
        StateMachineMovement.PlayerGet.PlayerRigidbody.linearVelocity = jumpForce;
        StateMachineMovement.ReusableData.SetJumpBuffer(0f);
        StateMachineMovement.ReusableData.SetCoyoteTime(0f);
    }

    public override void Exit() {
        base.Exit();
        SetBaseRotationData();
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
        if (_shouldKeepRotating) {
            RotateTowardsTargetRotation();
        }

        if (!IsMovingUp(0f)) {
            StateMachineMovement.ChangeState(StateMachineMovement.FallingState);
        }
    }

    private Vector3 GetJumpForceOnSlope(Vector3 jumpForce) {
        Vector3 capsuleColliderCenterInWorldSpace = StateMachineMovement.PlayerGet.ColliderUtility.CapsuleColliderData.Collider.bounds.center;

        Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

        if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, AirData.JumpData.JumpToGroundRayDistance, StateMachineMovement.PlayerGet.LayerData.GroundLayerMask, QueryTriggerInteraction.Ignore)) {
            float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);

            if (IsMovingUp()) {
                float forceModifier = AirData.JumpData.JumpForceModfierOnSlopeUpwards.Evaluate(groundAngle);

                jumpForce.x *= forceModifier;
                jumpForce.z *= forceModifier;
            }

            if (IsMovingDown()) {
                float forceModifier = AirData.JumpData.JumpForceModfierOnSlopeDownwards.Evaluate(groundAngle);

                jumpForce.y *= forceModifier;
            }
        }

        return jumpForce;
    }

    protected override void DoubleJump() {
        base.DoubleJump();
        Jump();
    }
}
