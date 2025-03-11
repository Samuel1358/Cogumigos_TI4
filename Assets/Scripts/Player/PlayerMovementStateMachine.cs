using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMovementStateMachine : StateMachine {
    public Player PlayerGet { get; private set; }
    public PlayerStateReusableData ReusableData { get; private set; }
    public PlayerIdlingState IdlingState { get; private set; }
    public PlayerDashingState DashingState { get; private set; }
    public PlayerWalkingState WalkingState { get; private set; }
    public PlayerRunningState RunningState { get; private set; }
    public PlayerSprintingState SprintingState { get; private set; }
    public PlayerLightStoppingState LightStoppingState { get; private set; }
    public PlayerMediumStoppingState MediumStoppingState { get; private set; }
    public PlayerHardStoppingState HardStoppingState { get; private set; }
    public PlayerJumpingState JumpingState { get; private set; }
    public PlayerFallingState FallingState { get; private set; }
    public PlayerLightLandingState LightLandingState { get; private set; }
    public PlayerHardLandingState HardLandingState { get; private set; }
    public PlayerRollingState RollingState { get; private set; }

    public PlayerMovementStateMachine(Player player) {
        PlayerGet = player;
        ReusableData = new PlayerStateReusableData();
        IdlingState = new PlayerIdlingState(this);
        DashingState = new PlayerDashingState(this);
        WalkingState = new PlayerWalkingState(this);
        RunningState = new PlayerRunningState(this);
        SprintingState = new PlayerSprintingState(this);
        LightStoppingState = new PlayerLightStoppingState(this);
        MediumStoppingState = new PlayerMediumStoppingState(this);
        HardStoppingState = new PlayerHardStoppingState(this);
        JumpingState = new PlayerJumpingState(this);
        FallingState = new PlayerFallingState(this);
        LightLandingState = new PlayerLightLandingState(this);
        HardLandingState = new PlayerHardLandingState(this);
        RollingState = new PlayerRollingState(this);
    }
}
