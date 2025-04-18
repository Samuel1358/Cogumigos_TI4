using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAirState : PlayerMovementState {
    protected float InitialJumpVelocity;
    Vector3 _gravityDir;
    public PlayerAirState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
        _gravityDir = new Vector3();
    }
    public override void Enter() {
        base.Enter();
        StartAnimation(StateMachineMovement.PlayerGet.AnimationData.AirborneParameterHash);
        ResetSpringState();
        SetUpJump();
    }

    public override void Exit() {
        base.Exit();
        StopAnimation(StateMachineMovement.PlayerGet.AnimationData.AirborneParameterHash);
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
        _gravityDir = new Vector3(0, AirData.Gravity, 0);
        StateMachineMovement.PlayerGet.PlayerRigidbody.linearVelocity += _gravityDir * Time.deltaTime;
    }

    protected override void AddInputActionsCallbacks() {
        base.AddInputActionsCallbacks();

        StateMachineMovement.PlayerGet.Input.PlayerActions.Jump.started += OnJumpStarted;
    }

    protected override void RemoveInputActionsCallbacks() {
        base.RemoveInputActionsCallbacks();

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
    protected virtual void ResetSpringState() {
        StateMachineMovement.ReusableData.ShouldSprint = false;
    }
    private void SetUpJump() {
        float timeToApex = AirData.JumpData.MaxJumpTime / 2.0f;
        AirData.SetGravity((-2.0f * AirData.JumpData.MaxJumpHeight) / MathF.Pow(timeToApex, 2.0f));
        InitialJumpVelocity = (2.0f * AirData.JumpData.MaxJumpHeight) / timeToApex;
    }
}
