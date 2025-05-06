using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerSprintingState : PlayerMovingState {

    private PlayerSprintData _sprintData;
    public PlayerSprintingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
        _sprintData = MovementData.SprintData;
    }
    public override void Enter() {
        base.Enter();
        StateMachineMovement.ReusableData.MovementSpeedModifier = _sprintData.SpeedModifier;
        StartAnimation(StateMachineMovement.PlayerGet.AnimationData.SprintParameterHash);
    }
    public override void Exit() {
        base.Exit();
        StopAnimation(StateMachineMovement.PlayerGet.AnimationData.SprintParameterHash);
    }
}