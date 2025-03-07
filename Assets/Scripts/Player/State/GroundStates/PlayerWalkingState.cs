using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerWalkingState : PlayerGroundState {
    public PlayerWalkingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
    }

    public override void Enter() {
        base.Enter();
        StateMachineMovement.ReusableData.MovementSpeedModifier = MovementData.WalkData.SpeedModifier;
        StateMachineMovement.ReusableData.CurrentJumpforce = AirData.JumpData.WeakForce;
    }

    protected override void OnWalkToggleStarted(InputAction.CallbackContext context) {
        base.OnWalkToggleStarted(context);

        StateMachineMovement.ChangeState(StateMachineMovement.RunningState);
    }
    protected override void OnMovementCanceled(InputAction.CallbackContext context) {
        StateMachineMovement.ChangeState(StateMachineMovement.LightStoppingState);
    }
}