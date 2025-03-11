using UnityEngine.InputSystem;

public class PlayerLandingState : PlayerGroundState {
    public PlayerLandingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context) {

    }
}
