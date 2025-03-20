using UnityEngine.InputSystem;

public class PlayerLandingState : PlayerGroundState {
    public PlayerLandingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
    }

    public override void Enter() {
        base.Enter();

        StartAnimation(StateMachineMovement.PlayerGet.AnimationData.LandingParameterHash);
    }

    public override void Exit() {
        base.Exit();

        StopAnimation(StateMachineMovement.PlayerGet.AnimationData.LandingParameterHash);
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context) {

    }
}
