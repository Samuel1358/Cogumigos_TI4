using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace Victor {
    public class PlayerIdlingState : PlayerGroundState {
        public PlayerIdlingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
        }

        public override void Enter() {
            base.Enter();

            StateMovementMachine.ReusableData.MovementSpeedModifier = 0f;

            ResetVelocity();
        }
        public override void Update() {
            base.Update();

            if (StateMovementMachine.ReusableData.MovementInput == Vector2.zero) {
                return;
            }

            OnMove();
        }
    }
}
