using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStoppingState : PlayerGroundState {
    public PlayerStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
    }
    public override void Enter() {
        base.Enter();

        StateMachineMovement.ReusableData.MovementSpeedModifier = 0f;

        StartAnimation(StateMachineMovement.PlayerGet.AnimationData.StoppingParameterHash);
    }

    public override void Exit() {
        base.Exit();
        StopAnimation(StateMachineMovement.PlayerGet.AnimationData.StoppingParameterHash);
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
        RotateTowardsTargetRotation();
        if (!IsMovingHorizontally()) {
            return;
        }
        DeceleationHorizontally();
    }
    protected override void AddInputActionsCallbacks() {
        base.AddInputActionsCallbacks();
        StateMachineMovement.PlayerGet.Input.PlayerActions.Move.started += OnMovementStarted;
    }
    protected override void RemoveInputActionsCallbacks() {
        base.RemoveInputActionsCallbacks();
        StateMachineMovement.PlayerGet.Input.PlayerActions.Move.started -= OnMovementStarted;
    }
    private void OnMovementStarted(InputAction.CallbackContext context) {
        OnMove();
    }

    public override void OnAnimationTransitionEvent() {
        StateMachineMovement.ChangeState(StateMachineMovement.IdlingState);
    }
    protected override void OnMovementCanceled(InputAction.CallbackContext context) {
    }
}
