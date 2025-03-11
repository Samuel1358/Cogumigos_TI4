using UnityEngine;

public class PlayerAirState : PlayerMovementState {
    public PlayerAirState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
    }
    public override void Enter() {
        base.Enter();
        ResetSpringState();
    }
    protected override void OnContactWithGround(Collider collider) {
        base.OnContactWithGround(collider);

        StateMachineMovement.ChangeState(StateMachineMovement.LightLandingState);
    }
    protected virtual void ResetSpringState() {
        StateMachineMovement.ReusableData.ShouldSprint = false;
    }
}
