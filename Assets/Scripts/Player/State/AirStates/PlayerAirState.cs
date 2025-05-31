using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAirState : PlayerMovementState {
    protected float InitialJumpVelocity;
    Vector3 _gravityDir;
    public PlayerAirState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
        _gravityDir = new Vector3();
    }
    public override void Enter() {
        base.Enter();
        StartAnimation(StateMachineMovement.PlayerGet.AnimationData.AirborneParameterHash);
        SetUpJump();
    }

    public override void Exit() {
        base.Exit();
        StopAnimation(StateMachineMovement.PlayerGet.AnimationData.AirborneParameterHash);
    }

    public override void Update() {
        base.Update();
        StateMachineMovement.ReusableData.SetCoyoteTime(StateMachineMovement.ReusableData.CoyoteTimeCount - Time.deltaTime);
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
        _gravityDir = new Vector3(0, StateMachineMovement.ReusableData.Gravity, 0);
        StateMachineMovement.PlayerGet.PlayerRigidbody.linearVelocity += _gravityDir * Time.deltaTime;
    }

    protected override void OnJumpStarted(InputAction.CallbackContext context) {
        base.OnJumpStarted(context);
        if (StateMachineMovement.ReusableData.CoyoteTimeCount <= 0 && (Cheats.instance != null) ? CheatInfinityJump() : StateMachineMovement.ReusableData.CanDoubleJump) {
            DoubleJump();
        }
    }

    private bool CheatInfinityJump()
    {
        return (!Cheats.instance.InfinityJump) ? StateMachineMovement.ReusableData.CanDoubleJump : true;
    }

    protected virtual void DoubleJump() {
        StateMachineMovement.ReusableData.DisableDoubleJump();
    }
    private void SetUpJump() {
        float timeToApex = AirData.JumpData.MaxJumpTime / 2.0f;
        StateMachineMovement.ReusableData.SetGravity((-2.0f * AirData.JumpData.MaxJumpHeight) / MathF.Pow(timeToApex, 2.0f));
        InitialJumpVelocity = (2.0f * AirData.JumpData.MaxJumpHeight) / timeToApex;
    }
}
