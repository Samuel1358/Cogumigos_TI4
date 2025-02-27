using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerRunningState : PlayerGroundState {
    public PlayerRunningState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
    }

    public override void Enter() {
        base.Enter();

        StateMovementMachine.ReusableData.MovementSpeedModifier = MovementData.RunData.SpeedModifier;
    }

    protected override void OnWalkToggleStarted(InputAction.CallbackContext context) {
        base.OnWalkToggleStarted(context);

        StateMovementMachine.ChangeState(StateMovementMachine.WalkingState);
    }
}