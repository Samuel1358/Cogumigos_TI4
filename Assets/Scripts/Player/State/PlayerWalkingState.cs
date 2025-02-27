using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerWalkingState : PlayerGroundState {
    public PlayerWalkingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
    }

    public override void Enter() {
        base.Enter();
        StateMovementMachine.ReusableData.MovementSpeedModifier = MovementData.WalkData.SpeedModifier;
    }

    protected override void OnWalkToggleStarted(InputAction.CallbackContext context) {
        base.OnWalkToggleStarted(context);

        StateMovementMachine.ChangeState(StateMovementMachine.RunningState);
    }
}