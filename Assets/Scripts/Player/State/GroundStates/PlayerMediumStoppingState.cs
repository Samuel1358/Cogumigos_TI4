public class PlayerMediumStoppingState : PlayerStoppingState {
    public PlayerMediumStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
    }
    public override void Enter() {
        base.Enter();
        StateMachineMovement.ReusableData.MovementDecelerationForce = MovementData.StopData.MediumDecelerationForce;
        StateMachineMovement.ReusableData.CurrentJumpforce = AirData.JumpData.MediumForce;
        StartAnimation(StateMachineMovement.PlayerGet.AnimationData.MediumStopParameterHash);
    }

    public override void Exit() {
        base.Exit();
        StopAnimation(StateMachineMovement.PlayerGet.AnimationData.MediumStopParameterHash);
    }
}
