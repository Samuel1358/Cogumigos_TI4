using UnityEngine;

public class CoguFollowState : CoguState
{
    // Constructor
    public CoguFollowState(CoguStateMachine stateMachine) : base(stateMachine, "Follow") { }

    // Interface Methods
    public override void Enter() 
    {
        base.Enter();

        stateMachine.cogu.JoinArmy(CoguArmy.instance.GetFollowTarget());
    }

    public override void Update() 
    {
        base.Update();

        stateMachine.cogu.Follow();
    }
}
