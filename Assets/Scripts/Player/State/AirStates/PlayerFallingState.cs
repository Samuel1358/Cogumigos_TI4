using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFallingState : PlayerAirState {
    private Vector3 _playerPositionOnEnter;

    public PlayerFallingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
    }

    public override void Enter() {
        base.Enter();

        StartAnimation(StateMachineMovement.PlayerGet.AnimationData.FallParameterHash);

        _playerPositionOnEnter = StateMachineMovement.PlayerGet.transform.position;

        StateMachineMovement.ReusableData.MovementSpeedModifier = AirData.JumpData.SpeedModifier;

        ResetVerticalVelocity();


        StateMachineMovement.ReusableData.SetGravity(StateMachineMovement.ReusableData.Gravity * AirData.FallData.FallMultiplier);
        if (IsThereGroundUnderneath()) {
            OnContactWithGround();
        }
    }

    public override void Exit() {
        base.Exit();

        StopAnimation(StateMachineMovement.PlayerGet.AnimationData.FallParameterHash);
    }

    private protected override void OnContactWithGround() {
        StateMachineMovement.ChangeState(StateMachineMovement.LightLandingState);
    }

    protected override void OnJumpStarted(InputAction.CallbackContext context) {
        base.OnJumpStarted(context);

        if (StateMachineMovement.ReusableData.CoyoteTimeCount > 0) {
            StateMachineMovement.ChangeState(StateMachineMovement.JumpingState);
        }
    }

    protected override void DoubleJump() {
        base.DoubleJump();
        StateMachineMovement.ChangeState(StateMachineMovement.JumpingState);
    }
}
