using UnityEngine;

public class CoguStateMachine : StateMachine
{
    public Cogu cogu { get; private set; }

    // States
    public CoguIdleState idleState { get; private set; }

    public CoguAttractState attractState { get; private set; }

    public CoguFollowState followState { get; private set; }

    public CoguThrowState throwState { get; private set; }

    public CoguInteractingState interactingState { get; private set; }

    // Constructor
    public CoguStateMachine(Cogu cogu)
    {
        this.cogu = cogu;
        this.idleState = new CoguIdleState(this);
        this.attractState = new CoguAttractState(this);
        this.followState = new CoguFollowState(this);
        this.throwState = new CoguThrowState(this);
    }
}
