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

    //public override void Exit() { }

    //public override void HandleInput() { }

    public override void Update() 
    {
        base.Update();

        stateMachine.cogu.Move();

        if (stateMachine.cogu.ArrivedDestination())
        {
            stateMachine.ChangeState(stateMachine.idleState);
        }
    }

    //public override void PhysicsUpdate() { }

    //public override void OnAnimationEnterEvent() { }

    //public override void OnAnimationExitEvent() { }

    //public override void OnAnimationTransitionEvent() { }

    //public override void OnTriggerEnter(Collider collider) { }

    //public override void OnTriggerExit(Collider collider) { }
}
