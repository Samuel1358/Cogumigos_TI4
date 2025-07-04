using UnityEngine;

public class PlayerGlideState : PlayerAirState {
    public PlayerGlideState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
    }

    public override void Enter() {
        base.Enter();

        StateMachineMovement.ReusableData.MovementSpeedModifier = 0f;
        ResetVelocity();
        StartAnimation(StateMachineMovement.PlayerGet.AnimationData.GlideParameterHash);
        GameIniciator.Instance.AudioManagerInstance.PlaySFX("AirTunnel");
    }

    public override void Update() {
        base.Update();

        StateMachineMovement.ReusableData.SetGravity(0f);
    }

    public override void Exit() {
        base.Exit();
        ResetVerticalVelocity();
        StopAnimation(StateMachineMovement.PlayerGet.AnimationData.GlideParameterHash);
        GameIniciator.Instance.AudioManagerInstance.StopSFX();
    }

    protected override void OnGlideVerify() {
        if (!StateMachineMovement.PlayerGet.ShouldGlide) {
            StateMachineMovement.ChangeState(StateMachineMovement.FallingState);
        }
    }
}
