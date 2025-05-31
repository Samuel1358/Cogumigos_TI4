using UnityEngine;

public class PlayerTrampolineJumpState : PlayerAirState
{
    private float trampolineForce;

    public PlayerTrampolineJumpState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    public void SetTrampolineForce(float force)
    {
        trampolineForce = force;
    }

    public override void Enter()
    {
        base.Enter();
        
        Vector3 currentVelocity = StateMachineMovement.PlayerGet.PlayerRigidbody.linearVelocity;
        currentVelocity.y = trampolineForce;
        StateMachineMovement.PlayerGet.PlayerRigidbody.linearVelocity = currentVelocity;
    }

    public override void Update()
    {
        base.Update();

        if (!IsMovingUp())
        {
            StateMachineMovement.ChangeState(StateMachineMovement.FallingState);
        }
    }

    protected override void DoubleJump() {
        base.DoubleJump();
        StateMachineMovement.ChangeState(StateMachineMovement.JumpingState);
    }
} 