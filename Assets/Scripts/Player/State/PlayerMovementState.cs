using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovementState : IState {
    protected PlayerMovementStateMachine StateMachineMovement;

    protected PlayerGroundedData MovementData;
    protected PlayerAirData AirData;

    public PlayerMovementState(PlayerMovementStateMachine playerMovementStateMachine) {
        StateMachineMovement = playerMovementStateMachine;

        MovementData = StateMachineMovement.PlayerGet.Data.GroundedData;
        AirData = StateMachineMovement.PlayerGet.Data.AirData;

        InitializeData();
    }

    private void InitializeData() {
        SetBaseRotationData();

    }

    public virtual void Enter() {
        AddInputActionsCallbacks();
    }

    public virtual void Exit() {
        RemoveInputActionsCallbacks();
    }

    public virtual void HandleInput() {
        ReadMovementInput();
    }

    public virtual void PhysicsUpdate() {
        Move();
    }

    public virtual void Update() {

    }

    private void ReadMovementInput() {
        StateMachineMovement.ReusableData.MovementInput = StateMachineMovement.PlayerGet.Input.PlayerActions.Move.ReadValue<Vector2>();
    }

    public virtual void OnAnimationEnterEvent() {

    }

    public virtual void OnAnimationExitEvent() {

    }

    public virtual void OnAnimationTransitionEvent() {

    }

    public void OnTriggerEnter(Collider collider) {
        if (StateMachineMovement.PlayerGet.LayerData.IsGroundLayer(collider.gameObject.layer)) {
            OnContactWithGround(collider);
            return;
        }
    }
    public void OnTriggerExit(Collider collider) {
        if (StateMachineMovement.PlayerGet.LayerData.IsGroundLayer(collider.gameObject.layer)) {
            OnContactWithGroundExited(collider);
        }
    }

    private void Move() {
        if (StateMachineMovement.ReusableData.MovementInput == Vector2.zero || StateMachineMovement.ReusableData.MovementSpeedModifier == 0f) {
            return;
        }
        Vector3 movementDirection = GetInputDirection();
        float targetRotationYAngle = Rotate(movementDirection);
        Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);
        float movementSpeed = GetMovementSpeed();
        Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();
        StateMachineMovement.PlayerGet.PlayerRigidbody.AddForce(targetRotationDirection * movementSpeed - currentPlayerHorizontalVelocity, ForceMode.VelocityChange);
    }

    private float Rotate(Vector3 direction) {
        float directionAngle = UpdateTargetRotation(direction);
        RotateTowardsTargetRotation();
        return directionAngle;
    }

    private float AddCameraRotationToAngle(float angle) {
        angle += StateMachineMovement.PlayerGet.MainCameraTransform.eulerAngles.y;
        if (angle > 360) {
            angle -= 360;
        }

        return angle;
    }

    private static float GetDirectionAngle(Vector3 direction) {
        float directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        if (directionAngle < 0f) {
            directionAngle += 360f;
        }

        return directionAngle;
    }

    public Vector3 GetInputDirection() {
        return new Vector3(StateMachineMovement.ReusableData.MovementInput.x, 0f, StateMachineMovement.ReusableData.MovementInput.y);
    }

    public float GetMovementSpeed() {
        return MovementData.BaseSpeed * StateMachineMovement.ReusableData.MovementSpeedModifier * StateMachineMovement.ReusableData.MovementOnSlopeSpeedModifier;
    }

    private Vector3 GetPlayerHorizontalVelocity() {
        Vector3 playerHorizontalVelocity = StateMachineMovement.PlayerGet.PlayerRigidbody.linearVelocity;
        playerHorizontalVelocity.y = 0;
        return playerHorizontalVelocity;
    }

    protected Vector3 GetPlayerVerticalVelocity() {
        return new Vector3(0f, StateMachineMovement.PlayerGet.PlayerRigidbody.linearVelocity.y, 0f);
    }

    protected void RotateTowardsTargetRotation() {
        float currentYAngle = StateMachineMovement.PlayerGet.PlayerRigidbody.rotation.eulerAngles.y;
        if (currentYAngle == StateMachineMovement.ReusableData.CurrentTargetRotation.y) {
            return;
        }
        float smoothedYAngle = Mathf.SmoothDampAngle(currentYAngle, StateMachineMovement.ReusableData.CurrentTargetRotation.y, ref StateMachineMovement.ReusableData.DampedTargetRotationCurrentVelocity.y, StateMachineMovement.ReusableData.TimeToReachTargetRoation.y - StateMachineMovement.ReusableData.DampedTargetRotationPassedTime.y);
        StateMachineMovement.ReusableData.DampedTargetRotationPassedTime.y += Time.deltaTime;

        Quaternion targetRotation = Quaternion.Euler(0f, smoothedYAngle, 0f);
        StateMachineMovement.PlayerGet.PlayerRigidbody.MoveRotation(targetRotation);
    }

    protected float UpdateTargetRotation(Vector3 direction, bool shouldConsiderCameraRotation = true) {
        float directionAngle = GetDirectionAngle(direction);
        if (shouldConsiderCameraRotation) {
            directionAngle = AddCameraRotationToAngle(directionAngle);
        }
        if (directionAngle != StateMachineMovement.ReusableData.CurrentTargetRotation.y) {
            UpdateTargetRotationData(directionAngle);
        }

        return directionAngle;
    }

    private void UpdateTargetRotationData(float targetAngle) {
        StateMachineMovement.ReusableData.CurrentTargetRotation.y = targetAngle;
        StateMachineMovement.ReusableData.DampedTargetRotationPassedTime.y = 0f;
    }

    protected Vector3 GetTargetRotationDirection(float targetAngle) {
        return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
    }

    protected void ResetVelocity() {
        StateMachineMovement.PlayerGet.PlayerRigidbody.linearVelocity = Vector3.zero;
    }

    protected void ResetVerticalVelocity() {
        Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();

        StateMachineMovement.PlayerGet.PlayerRigidbody.linearVelocity = playerHorizontalVelocity;
    }

    protected void SetBaseRotationData() {
        StateMachineMovement.ReusableData.RotationData = MovementData.BaseRotationData;
        StateMachineMovement.ReusableData.TimeToReachTargetRoation = StateMachineMovement.ReusableData.RotationData.TargetRotationReachTime;
    }

    protected virtual void OnContactWithGround(Collider collider) {

    }
    protected virtual void OnContactWithGroundExited(Collider collider) {
        
    }

    protected virtual void AddInputActionsCallbacks() {
        StateMachineMovement.PlayerGet.Input.PlayerActions.WalkTogle.started += OnWalkToggleStarted;
    }

    protected virtual void RemoveInputActionsCallbacks() {
        StateMachineMovement.PlayerGet.Input.PlayerActions.WalkTogle.started -= OnWalkToggleStarted;
    }
    protected void DeceleationHorizontally() {
        Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();
        StateMachineMovement.PlayerGet.PlayerRigidbody.AddForce(-playerHorizontalVelocity * StateMachineMovement.ReusableData.MovementDecelerationForce, ForceMode.Acceleration);
    }protected void DeceleationVertically() {
        Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();
        StateMachineMovement.PlayerGet.PlayerRigidbody.AddForce(-playerVerticalVelocity * StateMachineMovement.ReusableData.MovementDecelerationForce, ForceMode.Acceleration);
    }
    protected bool IsMovingHorizontally(float minimunMagnitude = 0.1f) {
        Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();
        Vector2 playerHorizontalMovement = new Vector2(playerHorizontalVelocity.x, playerHorizontalVelocity.z);
        return playerHorizontalMovement.magnitude > minimunMagnitude;
    }

    protected bool IsMovingUp(float minimumVelocity = 0.1f) {
        return GetPlayerVerticalVelocity().y > minimumVelocity;
    }

    protected bool IsMovingDown(float minimumVelocity = 0.1f) {
        return GetPlayerVerticalVelocity().y < -minimumVelocity;
    }

    #region Input methods
    protected virtual void OnWalkToggleStarted(InputAction.CallbackContext context) {
        StateMachineMovement.ReusableData.ShouldWalk = !StateMachineMovement.ReusableData.ShouldWalk;
    }
    #endregion
}
