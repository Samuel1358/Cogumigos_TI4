using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerGroundState : PlayerMovementState {

    private SlopeData slopeData;

    public PlayerGroundState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
        slopeData = StateMovementMachine.PlayerGet.ColliderUtility.SlopeData;
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();

        FloatCapsule();
    }

    private void FloatCapsule() {

        Vector3 capsuleColliderCenterInWorldSpace = StateMovementMachine.PlayerGet.ColliderUtility.CapsuleColliderData.Collider.bounds.center;

        Ray downardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

        if (Physics.Raycast(downardsRayFromCapsuleCenter, out RaycastHit Hit, slopeData.FloatRayDistance, StateMovementMachine.PlayerGet.LayerData.GroundLayerMask, QueryTriggerInteraction.Ignore)) {
            float groundAngle = Vector3.Angle(Hit.normal, -downardsRayFromCapsuleCenter.direction);
            float sloopeSpeedModifier = SetSlopeSpeedModifierOnAngle(groundAngle);

            if (sloopeSpeedModifier == 0f) {
                return;
            }

            float distancetoFloatPoint = StateMovementMachine.PlayerGet.ColliderUtility.CapsuleColliderData.ColliderCenterInLocalSpace.y * StateMovementMachine.PlayerGet.transform.localScale.y - Hit.distance;

            if (distancetoFloatPoint == 0f) {
                return;
            }
            float amountToLift = distancetoFloatPoint * slopeData.StepReachForce - GetPlayerVerticalVelocity().y;

            Vector3 liftForce = new Vector3(0f, amountToLift, 0f);

            StateMovementMachine.PlayerGet.PlayerRigidbody.AddForce(liftForce, ForceMode.VelocityChange);
        }
    }

    private float SetSlopeSpeedModifierOnAngle(float angle) {
        float slopeSpeedModifier = MovementData.SlopeSpeedAngles.Evaluate(angle);
        StateMovementMachine.ReusableData.MovementOnSlopeSpeedModifier = slopeSpeedModifier;
        return slopeSpeedModifier;
    }

    protected override void AddInputActionsCallbacks() {
        base.AddInputActionsCallbacks();
        StateMovementMachine.PlayerGet.Input.PlayerActions.Move.canceled += OnMovementCanceled;
    }

    protected override void RemoveInputActionsCallbacks() {
        base.RemoveInputActionsCallbacks();
        StateMovementMachine.PlayerGet.Input.PlayerActions.Move.canceled -= OnMovementCanceled;
    }

    protected virtual void OnMovementCanceled(InputAction.CallbackContext context) {
        StateMovementMachine.ChangeState(StateMovementMachine.IdlingState);
    }

    protected virtual void OnMove() {
        if (StateMovementMachine.ReusableData.ShouldWalk) {
            StateMovementMachine.ChangeState(StateMovementMachine.WalkingState);
            return;
        }
        StateMovementMachine.ChangeState(StateMovementMachine.RunningState);
    }
}
