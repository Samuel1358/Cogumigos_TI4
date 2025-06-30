using UnityEngine;

public class PlayerJumpingState : PlayerAirState {
    public PlayerJumpingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
    }

    public override void Enter() {
        base.Enter();
        StateMachineMovement.ReusableData.MovementSpeedModifier = 1f;
        AudioManager.Instance.PlaySFX("PlayerJump");
        Jump();
    }

    private void Jump() {
        Vector3 jumpForce = new Vector3(StateMachineMovement.PlayerGet.PlayerRigidbody.linearVelocity.x, InitialJumpVelocity, StateMachineMovement.PlayerGet.PlayerRigidbody.linearVelocity.z);
        StateMachineMovement.PlayerGet.PlayerRigidbody.linearVelocity = jumpForce;
        StateMachineMovement.ReusableData.SetJumpBuffer(0f);
        StateMachineMovement.ReusableData.SetCoyoteTime(0f);
    }

    public override void Exit() {
        base.Exit();
        StopAnimation(StateMachineMovement.PlayerGet.AnimationData.IsDoubleJump);
        SetBaseRotationData();
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
        RotateTowardsTargetRotation();

        if (!IsMovingUp(0f)) {
            StateMachineMovement.ChangeState(StateMachineMovement.FallingState);
        }
    }

    protected override void DoubleJump() {
        base.DoubleJump();
        StartAnimation(StateMachineMovement.PlayerGet.AnimationData.IsDoubleJump);
        Jump();
    }
}
