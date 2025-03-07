using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerSprintingState : PlayerGroundState {

    private PlayerSprintData _sprintData;
    private float _startTime;
    private bool _keepSprinting;
    private bool _shouldResetSprintState;
    public PlayerSprintingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
        _sprintData = MovementData.SprintData;
    }
    public override void Enter() {
        base.Enter();
        StateMachineMovement.ReusableData.MovementSpeedModifier = _sprintData.SpeedModifier;
        StateMachineMovement.ReusableData.CurrentJumpforce = AirData.JumpData.StrongForce;
        _shouldResetSprintState = true;
        _startTime = Time.time;
    }
    public override void Update() {
        base.Update();
        if (_keepSprinting) {
            return;
        }
        if (Time.time < _startTime + _sprintData.SprintToRunTime) {
            return;
        }
        StopSprinting();
    }
    public override void Exit() {
        base.Exit();
        _keepSprinting = false;
        if (_shouldResetSprintState) {
            _keepSprinting = false;
            StateMachineMovement.ReusableData.ShouldSprint = false;
        }
    }
    protected override void AddInputActionsCallbacks() {
        base.AddInputActionsCallbacks();
        StateMachineMovement.PlayerGet.Input.PlayerActions.Sprint.performed += OnSprintPerformed;
    }
    protected override void RemoveInputActionsCallbacks() {
        base.RemoveInputActionsCallbacks();
        StateMachineMovement.PlayerGet.Input.PlayerActions.Sprint.performed -= OnSprintPerformed;
    }

    protected override void OnFall() {
        _shouldResetSprintState = false;
        
        base.OnFall();
    }

    private void OnSprintPerformed(InputAction.CallbackContext context) {
        _keepSprinting = true;
        StateMachineMovement.ReusableData.ShouldSprint = true;
    }
    private void StopSprinting() {
        if (StateMachineMovement.ReusableData.MovementInput == Vector2.zero) {
            StateMachineMovement.ChangeState(StateMachineMovement.IdlingState);
            return;
        }
        StateMachineMovement.ChangeState(StateMachineMovement.RunningState);
    }
    protected override void OnMovementCanceled(InputAction.CallbackContext context) {
        StateMachineMovement.ChangeState(StateMachineMovement.HardStoppingState);
    }
    protected override void OnJumpStarted(InputAction.CallbackContext context) {
        _shouldResetSprintState = false;
        base.OnJumpStarted(context);
    }
}
