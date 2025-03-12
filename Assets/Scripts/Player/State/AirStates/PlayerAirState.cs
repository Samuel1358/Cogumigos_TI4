using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAirState : PlayerMovementState {
    public PlayerAirState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
    }
    public override void Enter() {
        base.Enter();
        ResetSpringState();
        StartAnimation(StateMachineMovement.PlayerGet.AnimationData.AirParameterHash);
    }

    public override void Exit() {
        base.Exit();
        StopAnimation(StateMachineMovement.PlayerGet.AnimationData.AirParameterHash);
    }

    protected override void AddInputActionsCallbacks() {
        base.AddInputActionsCallbacks();

        StateMachineMovement.PlayerGet.Input.PlayerActions.Glide.started += OnGlideStarted;

        StateMachineMovement.PlayerGet.Input.PlayerActions.Jump.started += OnJumpStarted;
    }

    protected override void RemoveInputActionsCallbacks() {
        base.RemoveInputActionsCallbacks();

        StateMachineMovement.PlayerGet.Input.PlayerActions.Glide.started -= OnGlideStarted;


        StateMachineMovement.PlayerGet.Input.PlayerActions.Jump.started -= OnJumpStarted;
    }

    private void OnJumpStarted(InputAction.CallbackContext context) {
        if (AirData.JumpData.CanDoubleJump) {
            AirData.JumpData.DisableDoubleJump();
            DoubleJump();
        }
    }

    protected virtual void DoubleJump() {
    }

    protected override void OnContactWithGround(Collider collider) {
        base.OnContactWithGround(collider);

        StateMachineMovement.ChangeState(StateMachineMovement.LightLandingState);
    }
    protected virtual void ResetSpringState() {
        StateMachineMovement.ReusableData.ShouldSprint = false;
    }

    protected virtual void OnGlideStarted(InputAction.CallbackContext context) {
        StateMachineMovement.ChangeState(StateMachineMovement.GlideState);
    }
}
