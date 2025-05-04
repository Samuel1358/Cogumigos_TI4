using UnityEngine;
using UnityEngine.InputSystem;

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
        
        StateMachineMovement.ReusableData.MovementSpeedModifier = AirData.JumpData.SpeedModifier;
        
        // Aplicar força vertical do trampolim
        Vector3 currentVelocity = StateMachineMovement.PlayerGet.PlayerRigidbody.linearVelocity;
        currentVelocity.y = trampolineForce;
        StateMachineMovement.PlayerGet.PlayerRigidbody.linearVelocity = currentVelocity;
        
        // Iniciar animação do pulo do trampolim
        StartAnimation(StateMachineMovement.PlayerGet.AnimationData.TrampolineJumpParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        
        StopAnimation(StateMachineMovement.PlayerGet.AnimationData.TrampolineJumpParameterHash);
    }

    public override void Update()
    {
        base.Update();

        // Mudar para o estado de queda quando o jogador começar a cair
        if (!IsMovingUp())
        {
            StateMachineMovement.ChangeState(StateMachineMovement.FallingState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
        // Não permitir pulo duplo durante o pulo do trampolim
    }
} 