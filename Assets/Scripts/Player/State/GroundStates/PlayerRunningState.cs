using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerRunningState : PlayerMovingState {
    private PlayerRunData _runData;
    private float _startTime;
    public PlayerRunningState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
        _runData = MovementData.RunData;
    }

    public override void Enter() {
        base.Enter();

        StateMachineMovement.ReusableData.MovementSpeedModifier = _runData.SpeedModifier;
        _startTime = Time.time;
        StartAnimation(StateMachineMovement.PlayerGet.AnimationData.RunParameterHash);
    }

    public override void Exit() {
        base.Exit();
        StopAnimation(StateMachineMovement.PlayerGet.AnimationData.RunParameterHash);
    }
    public override void Update() {
        base.Update();
        if (!StateMachineMovement.ReusableData.ShouldWalk) {
            return;
        }
        if (Time.time < _startTime + _runData.RunToWalkTime) {
            return;
        }
        StopRunning();
    }
    private void StopRunning() {
        if (StateMachineMovement.ReusableData.MovementInput == Vector2.zero) {
            StateMachineMovement.ChangeState(StateMachineMovement.IdlingState);
            return;
        }
        StateMachineMovement.ChangeState(StateMachineMovement.WalkingState);
    }

    protected override void OnWalkToggleStarted(InputAction.CallbackContext context) {
        base.OnWalkToggleStarted(context);

        StateMachineMovement.ChangeState(StateMachineMovement.WalkingState);
    }
    protected override void OnMovementCanceled(InputAction.CallbackContext context) {
        StateMachineMovement.ChangeState(StateMachineMovement.IdlingState);
    }
}