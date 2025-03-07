public class PlayerMediumStoppingState : PlayerStoppingState {
    public PlayerMediumStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
    }
    public override void Enter() {
        base.Enter();
        StateMachineMovement.ReusableData.MovementDecelerationForce = MovementData.StopData.MediumDecelerationForce;
        StateMachineMovement.ReusableData.CurrentJumpforce = AirData.JumpData.MediumForce;
    }
}
