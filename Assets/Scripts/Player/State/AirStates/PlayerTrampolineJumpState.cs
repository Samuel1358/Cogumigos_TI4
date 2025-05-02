using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTrampolineJumpState : PlayerAirState
{
    private float jumpDuration;
    private float jumpStartTime;
    private float trampolineForce;
    private AnimationCurve jumpCurve = new AnimationCurve(
        new Keyframe(0f, 0f),
        new Keyframe(0.5f, 1f),
        new Keyframe(1f, 0f)
    );

    public PlayerTrampolineJumpState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    public void SetTrampolineParameters(float force, float duration)
    {
        trampolineForce = force;
        jumpDuration = duration;
    }

    public override void Enter()
    {
        base.Enter();
        
        jumpStartTime = Time.time;
        StateMachineMovement.ReusableData.MovementSpeedModifier = AirData.JumpData.SpeedModifier;
        
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

        float elapsedTime = Time.time - jumpStartTime;
        float normalizedTime = elapsedTime / jumpDuration;

        if (normalizedTime >= 1f)
        {
            StateMachineMovement.ChangeState(StateMachineMovement.FallingState);
            return;
        }

        // Aplicar força vertical baseada na curva e na força do trampolim
        float jumpForce = jumpCurve.Evaluate(normalizedTime) * trampolineForce;
        Vector3 currentVelocity = StateMachineMovement.PlayerGet.PlayerRigidbody.linearVelocity;
        currentVelocity.y = jumpForce;
        StateMachineMovement.PlayerGet.PlayerRigidbody.linearVelocity = currentVelocity;
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