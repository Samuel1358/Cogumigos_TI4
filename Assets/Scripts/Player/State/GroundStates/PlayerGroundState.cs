using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerGroundState : PlayerMovementState {

    private SlopeData _slopeData;

    public PlayerGroundState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
        _slopeData = StateMachineMovement.PlayerGet.ColliderUtility.SlopeData;
    }

    public override void Enter() {
        base.Enter();

        StateMachineMovement.ReusableData.EnableDoubleJump();
        StateMachineMovement.PlayerGet.ColliderUtility.CapsuleColliderData.Collider.sharedMaterial = StateMachineMovement.PlayerGet.Data.GroundedData.PlayerPhysics;
        StartAnimation(StateMachineMovement.PlayerGet.AnimationData.GroundedParameterHash);
    }

    public override void Exit() {
        base.Exit();
        StateMachineMovement.ReusableData.SetCoyoteTime(AirData.JumpData.CoyoteTime);
        StopAnimation(StateMachineMovement.PlayerGet.AnimationData.GroundedParameterHash);
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

    protected override void AddInputActionsCallbacks() {
        base.AddInputActionsCallbacks();
        StateMachineMovement.PlayerGet.Input.PlayerActions.Move.canceled += OnMovementCanceled;
    }

    protected override void RemoveInputActionsCallbacks() {
        base.RemoveInputActionsCallbacks();
        StateMachineMovement.PlayerGet.Input.PlayerActions.Move.canceled -= OnMovementCanceled;
    }

    protected virtual void OnMovementCanceled(InputAction.CallbackContext context) {
        StateMachineMovement.ChangeState(StateMachineMovement.IdlingState);
    }

    protected virtual void OnMove() {
        StateMachineMovement.ChangeState(StateMachineMovement.SprintingState);
    }

    protected override void OnContactWithGroundExited() {
        base.OnContactWithGroundExited();

        Vector3 capsuleColliderCenterInWorldSpace = StateMachineMovement.PlayerGet.ColliderUtility.CapsuleColliderData.Collider.bounds.center;

        Ray donwardsRayFromCapsuleBottom = new Ray(capsuleColliderCenterInWorldSpace - StateMachineMovement.PlayerGet.ColliderUtility.CapsuleColliderData.ColliderVerticalExtents, Vector3.down);
        if (!Physics.Raycast(donwardsRayFromCapsuleBottom, out _, MovementData.GroundToFallRayDistance, StateMachineMovement.PlayerGet.LayerData.GroundLayerMask, QueryTriggerInteraction.Ignore)) {
            OnFall();
        }
    }

    protected virtual void OnFall() {
        StateMachineMovement.ChangeState(StateMachineMovement.FallingState);
    }

    protected override void OnJumpStarted(InputAction.CallbackContext context) {
        base.OnJumpStarted(context);
        StateMachineMovement.ChangeState(StateMachineMovement.JumpingState);
    }
}
