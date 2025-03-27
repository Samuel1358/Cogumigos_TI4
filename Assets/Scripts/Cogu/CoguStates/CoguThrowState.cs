using NUnit.Framework;
using UnityEngine;

public class CoguThrowState : CoguState
{
    // Constructor
    public CoguThrowState(CoguStateMachine stateMachine) : base(stateMachine, "Throw") { }

    // Interface Methods
    public override void Enter() 
    {
        base.Enter();

        stateMachine.cogu.Throw(CoguArmy.instance.GetTargetCursor());
    }

    public override void Update() 
    {
        base.Update();

        stateMachine.cogu.Move();

        if (stateMachine.cogu.ArrivedDestination())
        {
            stateMachine.ChangeState(stateMachine.idleState);
        }
    }
}
