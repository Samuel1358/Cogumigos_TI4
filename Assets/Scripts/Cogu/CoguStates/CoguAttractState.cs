using UnityEngine;

public class CoguAttractState : CoguState
{
    // Constructor
    public CoguAttractState(CoguStateMachine statemachine) : base(statemachine, "Attract") { }

    // Interface Methods
    public override void Enter() 
    {
        base.Enter();

        stateMachine.cogu.ArmyAttract(CoguArmy.instance.GetFollowTarget());
    }

    public override void Update() 
    {
        base.Update();

        stateMachine.cogu.Follow();

        CoguArmy.instance.UpdateArmy(stateMachine.cogu);
    }
}
