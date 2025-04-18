using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFallingState : PlayerAirState {
    private PlayerFallData _fallData;
    private Vector3 _playerPositionOnEnter;

    public PlayerFallingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
        _fallData = AirData.FallData;
    }

    public override void Enter() {
        base.Enter();

        StartAnimation(StateMachineMovement.PlayerGet.AnimationData.FallParameterHash);

        _playerPositionOnEnter = StateMachineMovement.PlayerGet.transform.position;

        StateMachineMovement.ReusableData.MovementSpeedModifier = AirData.JumpData.SpeedModifier;

        ResetVerticalVelocity();


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

        if (fallDistance < _fallData.MinimumDistanceToBeConsideredHardFall) {
            StateMachineMovement.ChangeState(StateMachineMovement.LightLandingState);

            return;
        }

        if (StateMachineMovement.ReusableData.ShouldWalk && !StateMachineMovement.ReusableData.ShouldSprint || StateMachineMovement.ReusableData.MovementInput == Vector2.zero) {
            StateMachineMovement.ChangeState(StateMachineMovement.HardLandingState);
            return;
        }
        StateMachineMovement.ChangeState(StateMachineMovement.RollingState);
    }

    protected override void DoubleJump() {
        base.DoubleJump();

        StateMachineMovement.ChangeState(StateMachineMovement.JumpingState);
    }

    protected virtual void OnGlidePerformed(InputAction.CallbackContext context) {
        //StateMachineMovement.ChangeState(StateMachineMovement.GlideState);
    }
}
