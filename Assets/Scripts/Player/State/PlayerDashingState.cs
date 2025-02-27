using System;
using UnityEngine;

public class PlayerDashingState : PlayerGroundState {
    private PlayerDashData _dashData;
    public PlayerDashingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
        _dashData = MovementData.DashData;
    }

    public override void Enter() {
        base.Enter();

        StateMovementMachine.ReusableData.MovementSpeedModifier = MovementData.DashData.SpeedModifier;

        AddForceToTransitionToStationaryState()
    }

    private void AddForceToTransitionToStationaryState() {
        if (StateMovementMachine.ReusableData.MovementInput != Vector2.zero) {
            return;
        }

        Vector3 characterRotationDirection = StateMovementMachine.PlayerGet.transform.forward;
        characterRotationDirection.y = 0f;
        StateMovementMachine.PlayerGet.PlayerRigidbody.linearVelocity = characterRotationDirection * GetMovementSpeed();
    }
}
