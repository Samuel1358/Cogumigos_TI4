using UnityEngine;

public class PlayerLightLandingState : PlayerLandingState {
    public PlayerLightLandingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
    }

    public override void Enter() {
        base.Enter();

        StateMachineMovement.ReusableData.MovementSpeedModifier = 0f;
        ResetVelocity();
    }

    public override void Update() {
        base.Update();

        if (StateMachineMovement.ReusableData.JumpBufferCount > 0) {
            StateMachineMovement.ChangeState(StateMachineMovement.JumpingState);
        }

        if (StateMachineMovement.ReusableData.MovementInput == Vector2.zero) {
            return;
        }
        OnMove();
    }
    public override void PhysicsUpdate() {
        base.PhysicsUpdate();

        if (!IsMovingHorizontally()) {
            return;
        }
        ResetVelocity();
    }

    public override void OnAnimationTransitionEvent() {
        StateMachineMovement.ChangeState(StateMachineMovement.IdlingState);
;    }
}
