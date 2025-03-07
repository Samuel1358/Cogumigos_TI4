using System;
using UnityEngine;

public class PlayerFallingState : PlayerAirState {
    private PlayerFallData _fallData;
    public PlayerFallingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
        _fallData = AirData.FallData;
    }

    public override void Enter() {
        base.Enter();

        StateMachineMovement.ReusableData.MovementSpeedModifier = 0f;

        ResetVerticalVelocity();
    }

    protected override void ResetSpringState() {
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();

        LimitVerticalVelocity();
    }

    private void LimitVerticalVelocity() {
        Vector3 playerVerticalvelocity = GetPlayerVerticalVelocity();

        if (playerVerticalvelocity.y >= -_fallData.FallSpeedLimit) {
            return;
        }
        Vector3 limitedVelocity = new Vector3(0f, -_fallData.FallSpeedLimit - playerVerticalvelocity.y, 0f);

        StateMachineMovement.PlayerGet.PlayerRigidbody.AddForce(limitedVelocity, ForceMode.VelocityChange);
    }
}
