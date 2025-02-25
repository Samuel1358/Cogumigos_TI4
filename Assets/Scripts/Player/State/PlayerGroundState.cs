using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Victor
{
    public class PlayerGroundState : PlayerMovementState {
        public PlayerGroundState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
        }


        protected override void AddInputActionsCallbacks() {
            StateMovementMachine.PlayerGet.Input.PlayerActions.Move.canceled += OnMovementCanceled;
        }

        protected override void RemoveInputActionsCallbacks() {
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
}
