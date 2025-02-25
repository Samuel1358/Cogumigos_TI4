using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Victor {
    public class PlayerMovementState : IState {
        protected PlayerMovementStateMachine StateMovementMachine;

        protected PlayerGroundedData MovementData;

        public PlayerMovementState(PlayerMovementStateMachine playerMovementStateMachine) {
            StateMovementMachine = playerMovementStateMachine;

            MovementData = StateMovementMachine.PlayerGet.Data.GroundedData;

            InitializeData();
        }

        private void InitializeData() {
            StateMovementMachine.ReusableData.TimeToReachTargetRoation = MovementData.BaseRotationData.TargetRotationReachTime;

        }

        public virtual void Enter() {
            //Debug.Log("State: " + StateMachine.PlayerGet);
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
            StateMovementMachine.ReusableData.MovementInput = StateMovementMachine.PlayerGet.Input.PlayerActions.Move.ReadValue<Vector2>();
        }

        private void Move() {
            if (StateMovementMachine.ReusableData.MovementInput == Vector2.zero || StateMovementMachine.ReusableData.MovementSpeedModifier == 0f) {
                if (StateMovementMachine.PlayerGet.PlayerRigidbody.linearVelocity.x + StateMovementMachine.PlayerGet.PlayerRigidbody.linearVelocity.z != 0) {
                    StateMovementMachine.PlayerGet.PlayerRigidbody.linearVelocity = new Vector3(0f, StateMovementMachine.PlayerGet.PlayerRigidbody.linearVelocity.y, 0f);
                    //Debug.Log("Zerando");
                }
                return;
            }
            Vector3 movementDirection = GetInputDirection();
            float targetRotationYAngle = Rotate(movementDirection);
            Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);
            float movementSpeed = GetMovementSpeed();
            Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();
            StateMovementMachine.PlayerGet.PlayerRigidbody.AddForce(targetRotationDirection * movementSpeed - currentPlayerHorizontalVelocity, ForceMode.VelocityChange);
        }

        private float Rotate(Vector3 direction) {
            float directionAngle = UpdateTargetRotation(direction);
            RotateTowardsTargetRotation();
            return directionAngle;
        }

        private float AddCameraRotationToAngle(float angle) {
            angle += StateMovementMachine.PlayerGet.MainCameraTransform.eulerAngles.y;
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

        private Vector3 GetInputDirection() {
            return new Vector3(StateMovementMachine.ReusableData.MovementInput.x, 0f, StateMovementMachine.ReusableData.MovementInput.y);
        }

        private float GetMovementSpeed() {
            return MovementData.BaseSpeed * StateMovementMachine.ReusableData.MovementSpeedModifier;
        }

        private Vector3 GetPlayerHorizontalVelocity() {
            Vector3 playerHorizontalVelocity = StateMovementMachine.PlayerGet.PlayerRigidbody.linearVelocity;
            playerHorizontalVelocity.y = 0;
            return playerHorizontalVelocity;
        }

        protected void RotateTowardsTargetRotation() {
            float currentYAngle = StateMovementMachine.PlayerGet.PlayerRigidbody.rotation.eulerAngles.y;
            if (currentYAngle == StateMovementMachine.ReusableData.CurrentTargetRotation.y) {
                return;
            }
            float smoothedYAngle = Mathf.SmoothDampAngle(currentYAngle, StateMovementMachine.ReusableData.CurrentTargetRotation.y, ref StateMovementMachine.ReusableData.DampedTargetRotationCurrentVelocity.y, StateMovementMachine.ReusableData.TimeToReachTargetRoation.y - StateMovementMachine.ReusableData.DampedTargetRotationPassedTime.y);
            StateMovementMachine.ReusableData.DampedTargetRotationPassedTime.y += Time.deltaTime;

            Quaternion targetRotation = Quaternion.Euler(0f, smoothedYAngle, 0f);
            StateMovementMachine.PlayerGet.PlayerRigidbody.MoveRotation(targetRotation);
        }

        private float UpdateTargetRotation(Vector3 direction, bool shouldConsiderCameraRotation = true) {
            float directionAngle = GetDirectionAngle(direction);
            if (shouldConsiderCameraRotation) {
                directionAngle = AddCameraRotationToAngle(directionAngle);
            }
            if (directionAngle != StateMovementMachine.ReusableData.CurrentTargetRotation.y) {
                UpdateTargetRotationData(directionAngle);
            }

            return directionAngle;
        }

        private void UpdateTargetRotationData(float targetAngle) {
            StateMovementMachine.ReusableData.CurrentTargetRotation.y = targetAngle;
            StateMovementMachine.ReusableData.DampedTargetRotationPassedTime.y = 0f;
        }

        private Vector3 GetTargetRotationDirection(float targetAngle) {
            return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }

        protected void ResetVelocity() {
            StateMovementMachine.PlayerGet.PlayerRigidbody.linearVelocity = Vector3.zero;
        }

        protected virtual void AddInputActionsCallbacks() {
            StateMovementMachine.PlayerGet.Input.PlayerActions.WalkTogle.started += OnWalkToggleStarted;
        }

        protected virtual void RemoveInputActionsCallbacks() {
            StateMovementMachine.PlayerGet.Input.PlayerActions.WalkTogle.started -= OnWalkToggleStarted;
        }
        #region Input methods
        protected virtual void OnWalkToggleStarted(InputAction.CallbackContext context) {
            StateMovementMachine.ReusableData.ShouldWalk = !StateMovementMachine.ReusableData.ShouldWalk;
        }
        #endregion
    }
}
