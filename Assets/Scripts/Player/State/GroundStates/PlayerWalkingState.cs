using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerWalkingState : PlayerMovingState {
    public PlayerWalkingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
    }

    public override void Enter() {
        base.Enter();
        StateMachineMovement.ReusableData.MovementSpeedModifier = MovementData.WalkData.SpeedModifier;
        StateMachineMovement.ReusableData.CurrentJumpforce = AirData.JumpData.WeakForce;
        StartAnimation(StateMachineMovement.PlayerGet.AnimationData.WalkParameterHash);
    }

    public override void Exit() {
        base.Exit();

        StopAnimation(StateMachineMovement.PlayerGet.AnimationData.WalkParameterHash);
    }

    protected override void OnWalkToggleStarted(InputAction.CallbackContext context) {
        base.OnWalkToggleStarted(context);

        StateMachineMovement.ChangeState(StateMachineMovement.RunningState);
    }
    protected override void OnMovementCanceled(InputAction.CallbackContext context) {
        StateMachineMovement.ChangeState(StateMachineMovement.LightStoppingState);
    }
}