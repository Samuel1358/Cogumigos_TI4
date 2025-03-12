using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRollingState : PlayerLandingState {
    private PlayerRollData _rollData;
    public PlayerRollingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
        _rollData = MovementData.RollData;
    }

    public override void Enter() {
        base.Enter();

        StateMachineMovement.ReusableData.MovementSpeedModifier = _rollData.SpeedModifier;

        StateMachineMovement.ReusableData.ShouldSprint = false;

        StartAnimation(StateMachineMovement.PlayerGet.AnimationData.RollParameterHash);
    }

    public override void Exit() {
        base.Exit();

        StopAnimation(StateMachineMovement.PlayerGet.AnimationData.RollParameterHash);
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();

        if (StateMachineMovement.ReusableData.MovementInput != Vector2.zero) {
            return;
        }
        RotateTowardsTargetRotation();
    }

    public override void OnAnimationTransitionEvent() {
        if (StateMachineMovement.ReusableData.MovementInput == Vector2.zero) {
            StateMachineMovement.ChangeState(StateMachineMovement.MediumStoppingState);
            return;
        }
        OnMove();
    }
    protected override void OnJumpStarted(InputAction.CallbackContext context) {
    }
}
