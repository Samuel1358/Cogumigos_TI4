using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class PlayerIdlingState : PlayerGroundState {
    public PlayerIdlingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
    }

    public override void Enter() {
        base.Enter();

        StateMachineMovement.ReusableData.MovementSpeedModifier = 0f;

        StateMachineMovement.ReusableData.CurrentJumpforce = AirData.JumpData.StationaryForce;

        ResetVelocity();
    }
    public override void Update() {
        base.Update();

        if (StateMachineMovement.ReusableData.MovementInput == Vector2.zero) {
            return;
        }

        OnMove();
    }
}
