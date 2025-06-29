public class PlayerMovementStateMachine : StateMachine {

    public Player PlayerGet { get; private set; }
    public PlayerStateReusableData ReusableData { get; private set; }
    public PlayerIdlingState IdlingState { get; private set; }
    public PlayerWalkingState WalkingState { get; private set; }
    public PlayerSprintingState SprintingState { get; private set; }
    public PlayerJumpingState JumpingState { get; private set; }
    public PlayerFallingState FallingState { get; private set; }
    public PlayerGlideState GlideState { get; private set; }
    public PlayerLightLandingState LightLandingState { get; private set; }
    public PlayerTrampolineJumpState TrampolineJumpState { get; private set; }

    public PlayerMovementStateMachine(Player player) {
        PlayerGet = player;
        ReusableData = new PlayerStateReusableData();
        IdlingState = new PlayerIdlingState(this);
        WalkingState = new PlayerWalkingState(this);
        SprintingState = new PlayerSprintingState(this);
        JumpingState = new PlayerJumpingState(this);
        FallingState = new PlayerFallingState(this);
        GlideState = new PlayerGlideState(this);
        LightLandingState = new PlayerLightLandingState(this);
        TrampolineJumpState = new PlayerTrampolineJumpState(this);
    }
}
