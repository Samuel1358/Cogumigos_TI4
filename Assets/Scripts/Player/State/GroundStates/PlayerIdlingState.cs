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

        ResetVelocity();

        StartAnimation(StateMachineMovement.PlayerGet.AnimationData.IdleParameterHash);
    }

    public override void Exit() {
        base.Exit();
        StopAnimation(StateMachineMovement.PlayerGet.AnimationData.IdleParameterHash);
    }

    public override void Update() {
        base.Update();

        if (StateMachineMovement.ReusableData.MovementInput == Vector2.zero) {
            return;
        }

        OnMove();
    }
    public override void PhysicsUpdate() {
        base.PhysicsUpdate();

        if (!IsMovingHorizontally()) {
            return;
        }
        ResetVelocity();
    }
}
