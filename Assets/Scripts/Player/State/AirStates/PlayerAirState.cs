using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAirState : PlayerMovementState {
    public PlayerAirState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
    }
    public override void Enter() {
        base.Enter();
        StartAnimation(StateMachineMovement.PlayerGet.AnimationData.AirborneParameterHash);
        ResetSpringState();
    }

    public override void Exit() {
        base.Exit();
        StopAnimation(StateMachineMovement.PlayerGet.AnimationData.AirborneParameterHash);
    }

    public override void Update() {
        base.Update();


    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
    }

    protected override void AddInputActionsCallbacks() {
        base.AddInputActionsCallbacks();

        StateMachineMovement.PlayerGet.Input.PlayerActions.Glide.performed += OnGlidePerformed;

        StateMachineMovement.PlayerGet.Input.PlayerActions.Jump.started += OnJumpStarted;
    }

    protected override void RemoveInputActionsCallbacks() {
        base.RemoveInputActionsCallbacks();

        StateMachineMovement.PlayerGet.Input.PlayerActions.Glide.performed -= OnGlidePerformed;


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

    private protected override void OnContactWithGround() {
        base.OnContactWithGround();

        StateMachineMovement.ChangeState(StateMachineMovement.LightLandingState);
    }
    protected virtual void ResetSpringState() {
        StateMachineMovement.ReusableData.ShouldSprint = false;
    }

    protected virtual void OnGlidePerformed(InputAction.CallbackContext context) {
        StateMachineMovement.ChangeState(StateMachineMovement.GlideState);
    }
}
