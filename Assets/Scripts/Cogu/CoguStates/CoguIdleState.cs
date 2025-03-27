using UnityEngine;

public class CoguIdleState : CoguState
{
    // Constructor
    public CoguIdleState(CoguStateMachine stateMachine) : base(stateMachine, "Idle") { }

    // Interface Methods
    public override void Enter() 
    {
        base.Enter();

        stateMachine.cogu.Stop();
    }

    public override void Update() 
    {
        base.Update();

        stateMachine.cogu.Chillin();
    }
}
