using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGlideState : PlayerAirState {
    private PlayerGlideData _glideData;
    public PlayerGlideState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
        _glideData = AirData.GlideData;
    }

    public override void Enter() {
        base.Enter();
        StateMachineMovement.ReusableData.MovementSpeedModifier = _glideData.SpeedModifier;
        ResetVerticalVelocity();
        StartAnimation(StateMachineMovement.PlayerGet.AnimationData.GlideParameterHash);
    }

    public override void Exit() {
        base.Exit();
        StopAnimation(StateMachineMovement.PlayerGet.AnimationData.GlideParameterHash);
    }

    public override void Update() {
        base.Update();
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();

        LimitVerticalVelocity();
    }

    protected override void AddInputActionsCallbacks() {
        base.AddInputActionsCallbacks();
        StateMachineMovement.PlayerGet.Input.PlayerActions.Glide.canceled += OnGlideCanceled;
    }

    protected override void RemoveInputActionsCallbacks() {
        base.RemoveInputActionsCallbacks();
        StateMachineMovement.PlayerGet.Input.PlayerActions.Glide.canceled -= OnGlideCanceled;
    }

    private void OnGlideCanceled(InputAction.CallbackContext context) {
        StateMachineMovement.ChangeState(StateMachineMovement.FallingState);
    }

    private void LimitVerticalVelocity() {
        Vector3 playerVerticalvelocity = GetPlayerVerticalVelocity();

        if (playerVerticalvelocity.y >= -_glideData.FallSpeedLimit) {
            return;
        }
        Vector3 limitedVelocity = new Vector3(0f, -_glideData.FallSpeedLimit - playerVerticalvelocity.y, 0f);

        StateMachineMovement.PlayerGet.PlayerRigidbody.AddForce(limitedVelocity, ForceMode.VelocityChange);
    }
}
