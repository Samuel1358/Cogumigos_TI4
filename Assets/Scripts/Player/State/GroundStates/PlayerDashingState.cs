using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDashingState : PlayerGroundState {
    private PlayerDashData _dashData;
    private float _startTime;
    private int _consecutiveDashesUsed;
    private bool _shouldKeepRotating;
    public PlayerDashingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
        _dashData = MovementData.DashData;
    }

    public override void Enter() {
        base.Enter();

        StartAnimation(StateMachineMovement.PlayerGet.AnimationData.DashParameterHash);

        StateMachineMovement.ReusableData.MovementSpeedModifier = _dashData.SpeedModifier;

        StateMachineMovement.ReusableData.RotationData = _dashData.RotationData;
        StateMachineMovement.ReusableData.CurrentJumpforce = AirData.JumpData.StrongForce;

        Dash();
        _shouldKeepRotating = StateMachineMovement.ReusableData.MovementInput == Vector2.zero;

        UpdateConsecutiveDashes();
        _startTime = Time.time;
    }
    public override void PhysicsUpdate() {
        base.PhysicsUpdate();
        if (!_shouldKeepRotating) {
            return;
        }
        RotateTowardsTargetRotation();
    }
    public override void Exit() {
        base.Exit();

        SetBaseRotationData();

        StopAnimation(StateMachineMovement.PlayerGet.AnimationData.DashParameterHash);
    }
    protected override void AddInputActionsCallbacks() {
        base.AddInputActionsCallbacks();

        StateMachineMovement.PlayerGet.Input.PlayerActions.Move.performed += OnMovementPerformed;
    }
    protected override void RemoveInputActionsCallbacks() {
        base.RemoveInputActionsCallbacks();

        StateMachineMovement.PlayerGet.Input.PlayerActions.Move.performed -= OnMovementPerformed;
    }
    public override void OnAnimationTransitionEvent() {
        if (StateMachineMovement.ReusableData.MovementInput == Vector2.zero) {
            StateMachineMovement.ChangeState(StateMachineMovement.HardStoppingState);
            return;
        }
        StateMachineMovement.ChangeState(StateMachineMovement.SprintingState);
    }
    private void UpdateConsecutiveDashes() {
        if (!IsConsecutive()) {
            _consecutiveDashesUsed = 0;
        }
        ++_consecutiveDashesUsed;
        if (_consecutiveDashesUsed == _dashData.ConsecutiveDashesLimitAmout) {
            _consecutiveDashesUsed = 0;
            StateMachineMovement.PlayerGet.Input.DisableActionFor(StateMachineMovement.PlayerGet.Input.PlayerActions.Dash, _dashData.DashLimitReachedCooldown);
        }
    }

    private bool IsConsecutive() {
        return Time.time < _startTime + _dashData.TimeToBeConsideredConsecutive;
    }

    private void Dash() {
        Vector3 dashDirection = StateMachineMovement.PlayerGet.transform.forward;
        dashDirection.y = 0f;
        UpdateTargetRotation(dashDirection, false);

        if (StateMachineMovement.ReusableData.MovementInput != Vector2.zero) {
            UpdateTargetRotation(GetInputDirection());
            dashDirection = GetTargetRotationDirection(StateMachineMovement.ReusableData.CurrentTargetRotation.y);
        }

        StateMachineMovement.PlayerGet.PlayerRigidbody.linearVelocity = dashDirection * GetMovementSpeed(false);
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context) {
        
    }
    protected override void OnDashStarted(InputAction.CallbackContext context) {

    }
    private void OnMovementPerformed(InputAction.CallbackContext context) {
        _shouldKeepRotating = true;
    }
}
