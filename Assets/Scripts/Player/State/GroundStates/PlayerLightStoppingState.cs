public class PlayerLightStoppingState : PlayerStoppingState {
    public PlayerLightStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
    }
    public override void Enter() {
        base.Enter();
        StateMachineMovement.ReusableData.MovementDecelerationForce = MovementData.StopData.LightDecelerationForce;
        StateMachineMovement.ReusableData.CurrentJumpforce = AirData.JumpData.WeakForce;
    }
}
