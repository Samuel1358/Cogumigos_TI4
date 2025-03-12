using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerGroundState : PlayerMovementState {

    private SlopeData _slopeData;

    public PlayerGroundState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
        _slopeData = StateMachineMovement.PlayerGet.ColliderUtility.SlopeData;
    }

    public override void Enter() {
        base.Enter();

        AirData.JumpData.EnableDoubleJump();

        StartAnimation(StateMachineMovement.PlayerGet.AnimationData.GroundedParameterHash);

        UpdateShouldSprintState();
    }

    public override void Exit() {
        base.Exit();

        StopAnimation(StateMachineMovement.PlayerGet.AnimationData.GroundedParameterHash);
    }

    private void UpdateShouldSprintState() {
        if (!StateMachineMovement.ReusableData.ShouldSprint) {
            return;
        }
        if (StateMachineMovement.ReusableData.MovementInput != Vector2.zero) {
            return;
        }
        StateMachineMovement.ReusableData.ShouldSprint = false;
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();

        FloatCapsule();
    }

    private void FloatCapsule() {

        Vector3 capsuleColliderCenterInWorldSpace = StateMachineMovement.PlayerGet.ColliderUtility.CapsuleColliderData.Collider.bounds.center;

        Ray downardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

        if (Physics.Raycast(downardsRayFromCapsuleCenter, out RaycastHit Hit, _slopeData.FloatRayDistance, StateMachineMovement.PlayerGet.LayerData.GroundLayerMask, QueryTriggerInteraction.Ignore)) {
            float groundAngle = Vector3.Angle(Hit.normal, -downardsRayFromCapsuleCenter.direction);
            float sloopeSpeedModifier = SetSlopeSpeedModifierOnAngle(groundAngle);

            if (sloopeSpeedModifier == 0f) {
                return;
            }

            float distancetoFloatPoint = StateMachineMovement.PlayerGet.ColliderUtility.CapsuleColliderData.ColliderCenterInLocalSpace.y * StateMachineMovement.PlayerGet.transform.localScale.y - Hit.distance;

            if (distancetoFloatPoint == 0f) {
                return;
            }
            float amountToLift = distancetoFloatPoint * _slopeData.StepReachForce - GetPlayerVerticalVelocity().y;

            Vector3 liftForce = new Vector3(0f, amountToLift, 0f);

            StateMachineMovement.PlayerGet.PlayerRigidbody.AddForce(liftForce, ForceMode.VelocityChange);
        }
    }

    private float SetSlopeSpeedModifierOnAngle(float angle) {
        float slopeSpeedModifier = MovementData.SlopeSpeedAngles.Evaluate(angle);
        StateMachineMovement.ReusableData.MovementOnSlopeSpeedModifier = slopeSpeedModifier;
        return slopeSpeedModifier;
    }

    private bool IsThereGroundUnderneath() {
        BoxCollider groundCheckCollider = StateMachineMovement.PlayerGet.ColliderUtility.TriggerColliderData.GroundCheckCollider;
        Vector3 groundColliderCenterInWorldSpace = groundCheckCollider.bounds.center;
        Collider[] overlapGroundCollider = Physics.OverlapBox(groundColliderCenterInWorldSpace, StateMachineMovement.PlayerGet.ColliderUtility.TriggerColliderData.GroundCheckColliderExtents, groundCheckCollider.transform.rotation, StateMachineMovement.PlayerGet.LayerData.GroundLayerMask, QueryTriggerInteraction.Ignore);

        return overlapGroundCollider.Length > 0;
    }

    protected override void AddInputActionsCallbacks() {
        base.AddInputActionsCallbacks();
        StateMachineMovement.PlayerGet.Input.PlayerActions.Move.canceled += OnMovementCanceled;

        StateMachineMovement.PlayerGet.Input.PlayerActions.Dash.started += OnDashStarted;

        StateMachineMovement.PlayerGet.Input.PlayerActions.Jump.started += OnJumpStarted;
    }

    protected override void RemoveInputActionsCallbacks() {
        base.RemoveInputActionsCallbacks();
        StateMachineMovement.PlayerGet.Input.PlayerActions.Move.canceled -= OnMovementCanceled;

        StateMachineMovement.PlayerGet.Input.PlayerActions.Dash.started -= OnDashStarted;

        StateMachineMovement.PlayerGet.Input.PlayerActions.Jump.started -= OnJumpStarted;
    }

    protected virtual void OnMovementCanceled(InputAction.CallbackContext context) {
        StateMachineMovement.ChangeState(StateMachineMovement.IdlingState);
    }

    protected virtual void OnMove() {
        if (StateMachineMovement.ReusableData.ShouldSprint) {
            StateMachineMovement.ChangeState(StateMachineMovement.SprintingState);

            return;
        }
        if (StateMachineMovement.ReusableData.ShouldWalk) {
            StateMachineMovement.ChangeState(StateMachineMovement.WalkingState);
            return;
        }
        StateMachineMovement.ChangeState(StateMachineMovement.RunningState);
    }

    protected override void OnContactWithGroundExited(Collider collider) {
        base.OnContactWithGroundExited(collider);

        if (IsThereGroundUnderneath()) {
            return;

        }

        Vector3 capsuleColliderCenterInWorldSpace = StateMachineMovement.PlayerGet.ColliderUtility.CapsuleColliderData.Collider.bounds.center;

        Ray donwardsRayFromCapsuleBottom = new Ray(capsuleColliderCenterInWorldSpace - StateMachineMovement.PlayerGet.ColliderUtility.CapsuleColliderData.ColliderVerticalExtents, Vector3.down);
        if (!Physics.Raycast(donwardsRayFromCapsuleBottom, out _, MovementData.GroundToFallRayDistance, StateMachineMovement.PlayerGet.LayerData.GroundLayerMask, QueryTriggerInteraction.Ignore)) {
            OnFall();
        }
    }

    protected virtual void OnFall() {
        StateMachineMovement.ChangeState(StateMachineMovement.FallingState);
    }

    protected virtual void OnDashStarted(InputAction.CallbackContext context) {
        StateMachineMovement.ChangeState(StateMachineMovement.DashingState);
    }

    protected virtual void OnJumpStarted(InputAction.CallbackContext context) {
        StateMachineMovement.ChangeState(StateMachineMovement.JumpingState);
    }
}
