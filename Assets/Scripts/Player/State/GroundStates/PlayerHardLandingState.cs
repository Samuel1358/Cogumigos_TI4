using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHardLandingState : PlayerLandingState {
    public PlayerHardLandingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
    }

    public override void Enter() {
        base.Enter();

        StateMachineMovement.PlayerGet.Input.PlayerActions.Move.Disable();

        StateMachineMovement.ReusableData.MovementSpeedModifier = 0f;

        ResetVelocity();
    }

    public override void Exit() {
        base.Exit();

        StateMachineMovement.PlayerGet.Input.PlayerActions.Move.Enable();
    }
    public override void PhysicsUpdate() {
        base.PhysicsUpdate();

        if (!IsMovingHorizontally()) {
            return;
        }
        ResetVelocity();
    }

    public override void OnAnimationTransitionEvent() {
        StateMachineMovement.ChangeState(StateMachineMovement.IdlingState);
    }

    public override void OnAnimationExitEvent() {
        StateMachineMovement.PlayerGet.Input.PlayerActions.Move.Enable();
    }

    protected override void AddInputActionsCallbacks() {
        base.AddInputActionsCallbacks();
        StateMachineMovement.PlayerGet.Input.PlayerActions.Move.started += OnMovementStarted;
    }

    protected override void RemoveInputActionsCallbacks() {
        base.RemoveInputActionsCallbacks();

        StateMachineMovement.PlayerGet.Input.PlayerActions.Move.started -= OnMovementStarted;
    }

    protected override void OnMove() {
        if (StateMachineMovement.ReusableData.ShouldWalk) {
            return;
        }
        StateMachineMovement.ChangeState(StateMachineMovement.RunningState);
    }
    protected override void OnJumpStarted(InputAction.CallbackContext context) {
        
    }
    private void OnMovementStarted(InputAction.CallbackContext context) {
        OnMove();
    }
}
