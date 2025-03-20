public class PlayerHardStoppingState : PlayerStoppingState {
    public PlayerHardStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
    }
    public override void Enter() {
        base.Enter();
        StateMachineMovement.ReusableData.MovementDecelerationForce = MovementData.StopData.HardDecelerationForce;
        StateMachineMovement.ReusableData.CurrentJumpforce = AirData.JumpData.StrongForce;
        StartAnimation(StateMachineMovement.PlayerGet.AnimationData.HardStopParameterHash);
    }

    public override void Exit() {
        base.Exit();
        StopAnimation(StateMachineMovement.PlayerGet.AnimationData.HardStopParameterHash);
    }
    protected override void OnMove() {
        if (StateMachineMovement.ReusableData.ShouldWalk) {
            return;
        }
        StateMachineMovement.ChangeState(StateMachineMovement.RunningState);
    }
}
