using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFallingState : PlayerAirState {
    private Vector3 _playerPositionOnEnter;

    public PlayerFallingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
    }

    public override void Enter() {
        base.Enter();

        StartAnimation(StateMachineMovement.PlayerGet.AnimationData.FallParameterHash);

        _playerPositionOnEnter = StateMachineMovement.PlayerGet.transform.position;

        StateMachineMovement.ReusableData.MovementSpeedModifier = AirData.JumpData.SpeedModifier;

        ResetVerticalVelocity();


        StateMachineMovement.ReusableData.SetGravity(StateMachineMovement.ReusableData.Gravity * AirData.FallData.FallMultiplier);
        if (IsThereGroundUnderneath()) {
            OnContactWithGround();
        }
    }

    public override void Exit() {
        base.Exit();

        StopAnimation(StateMachineMovement.PlayerGet.AnimationData.FallParameterHash);
    }

    protected override void AddInputActionsCallbacks() {
        base.AddInputActionsCallbacks();

        StateMachineMovement.PlayerGet.Input.PlayerActions.Glide.performed += OnGlidePerformed;
    }

    protected override void RemoveInputActionsCallbacks() {
        base.RemoveInputActionsCallbacks();

        StateMachineMovement.PlayerGet.Input.PlayerActions.Glide.performed -= OnGlidePerformed;
    }

    protected override void ResetSpringState() {
    }

    private protected override void OnContactWithGround() {
        float fallDistance = _playerPositionOnEnter.y - StateMachineMovement.PlayerGet.transform.position.y;

        if (fallDistance < AirData.FallData.MinimumDistanceToBeConsideredHardFall) {
            StateMachineMovement.ChangeState(StateMachineMovement.LightLandingState);

            return;
        }

        if (StateMachineMovement.ReusableData.ShouldWalk && !StateMachineMovement.ReusableData.ShouldSprint || StateMachineMovement.ReusableData.MovementInput == Vector2.zero) {
            StateMachineMovement.ChangeState(StateMachineMovement.HardLandingState);
            return;
        }
        StateMachineMovement.ChangeState(StateMachineMovement.RollingState);
    }

    protected override void OnJumpStarted(InputAction.CallbackContext context) {
        base.OnJumpStarted(context);

        if (StateMachineMovement.ReusableData.CoyoteTimeCount > 0) {
            StateMachineMovement.ChangeState(StateMachineMovement.JumpingState);
        }
    }

    protected override void DoubleJump() {
        base.DoubleJump();
        StateMachineMovement.ChangeState(StateMachineMovement.JumpingState);
    }

    protected void OnGlidePerformed(InputAction.CallbackContext context) {
    }
}
