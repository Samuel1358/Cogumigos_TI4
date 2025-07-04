using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFallingState : PlayerAirState {

    public PlayerFallingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
    }

    public override void Enter() {
        base.Enter();

        StartAnimation(StateMachineMovement.PlayerGet.AnimationData.FallParameterHash);

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

    protected override void DoubleJump() {
        base.DoubleJump();
        StartAnimation(StateMachineMovement.PlayerGet.AnimationData.IsDoubleJump);
        StateMachineMovement.ChangeState(StateMachineMovement.JumpingState);
    }
}
