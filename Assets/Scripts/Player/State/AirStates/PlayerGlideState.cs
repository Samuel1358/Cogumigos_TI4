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
        Debug.Log("Gliding");
        StateMachineMovement.ReusableData.MovementSpeedModifier = 0f;
        // gravidade? StateMachineMovement.ReusableData.SetGravity(0f);
        ResetVerticalVelocity();
        StartAnimation(StateMachineMovement.PlayerGet.AnimationData.GlideParameterHash);
        AudioManager.Instance.PlaySFX("AirTunnel");
    }

    public override void Exit() {
        base.Exit();
        Debug.Log("No more gliding");
        StopAnimation(StateMachineMovement.PlayerGet.AnimationData.GlideParameterHash);
        AudioManager.Instance.StopSFX();
    }

    protected override void OnGlideVerify() {
        if (!StateMachineMovement.PlayerGet.ShouldGlide) {
            StateMachineMovement.ChangeState(StateMachineMovement.FallingState);
        }
    }
}
