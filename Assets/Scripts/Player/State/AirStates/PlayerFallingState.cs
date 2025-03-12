using System;
using UnityEngine;

public class PlayerFallingState : PlayerAirState {
    private PlayerFallData _fallData;
    private Vector3 _playerPositionOnEnter;
    public PlayerFallingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
        _fallData = AirData.FallData;
    }

    public override void Enter() {
        base.Enter();

        _playerPositionOnEnter = StateMachineMovement.PlayerGet.transform.position;

        StateMachineMovement.ReusableData.MovementSpeedModifier = 0f;

        ResetVerticalVelocity();

        StartAnimation(StateMachineMovement.PlayerGet.AnimationData.FallParameterHash);
    }

    public override void Exit() {
        base.Exit();

        StopAnimation(StateMachineMovement.PlayerGet.AnimationData.FallParameterHash);
    }

    protected override void ResetSpringState() {
    }

    protected override void OnContactWithGround(Collider collider) {
        float fallDistance = _playerPositionOnEnter.y - StateMachineMovement.PlayerGet.transform.position.y;

        if (fallDistance < _fallData.MinimumDistanceToBeConsideredHardFall) {
            StateMachineMovement.ChangeState(StateMachineMovement.LightLandingState);

            return;
        }

        if (StateMachineMovement.ReusableData.ShouldWalk && !StateMachineMovement.ReusableData.ShouldSprint || StateMachineMovement.ReusableData.MovementInput == Vector2.zero) {
            StateMachineMovement.ChangeState(StateMachineMovement.HardLandingState);
            return;
        }
        StateMachineMovement.ChangeState(StateMachineMovement.RollingState);
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();

        LimitVerticalVelocity();
    }

    private void LimitVerticalVelocity() {
        Vector3 playerVerticalvelocity = GetPlayerVerticalVelocity();

        if (playerVerticalvelocity.y >= -_fallData.FallSpeedLimit) {
            return;
        }
        Vector3 limitedVelocity = new Vector3(0f, -_fallData.FallSpeedLimit - playerVerticalvelocity.y, 0f);

        StateMachineMovement.PlayerGet.PlayerRigidbody.AddForce(limitedVelocity, ForceMode.VelocityChange);
    }

    protected override void DoubleJump() {
        base.DoubleJump();

        StateMachineMovement.ChangeState(StateMachineMovement.JumpingState);
    }
}
