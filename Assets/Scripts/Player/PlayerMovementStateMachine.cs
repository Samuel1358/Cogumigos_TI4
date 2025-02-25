using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Victor {
    public class PlayerMovementStateMachine : StateMachine {
        public Player PlayerGet { get; private set; }
        public PlayerStateReusableData ReusableData { get; private set; }
        public PlayerIdlingState IdlingState { get; private set; }
        public PlayerWalkingState WalkingState { get; private set; }
        public PlayerRunningState RunningState { get; private set; }
        public PlayerSprintingState SprintingState { get; private set; }

        public PlayerMovementStateMachine(Player player) {
            PlayerGet = player;
            ReusableData = new PlayerStateReusableData();
            IdlingState = new PlayerIdlingState(this);
            WalkingState = new PlayerWalkingState(this);
            RunningState = new PlayerRunningState(this);
            SprintingState = new PlayerSprintingState(this);
        }
    }
}
