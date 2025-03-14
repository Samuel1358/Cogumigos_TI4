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

        stateMachine.cogu.SetTarget(CoguArmy.instance.GetTargetCursor().position);
        stateMachine.cogu.SetTargetFollow(null);

        stateMachine.cogu.GetAgent().enabled = true;
        stateMachine.cogu.GetAgent().stoppingDistance = 0f;
        stateMachine.cogu.GetAgent().speed = stateMachine.cogu.GetThrowSpd();
    }

    //public override void Exit() { }

    //public override void HandleInput() { }

    public override void Update() 
    {
        base.Update();

        if (stateMachine.cogu.GetTarget() != null)
            stateMachine.cogu.GetAgent().SetDestination(stateMachine.cogu.GetTarget());
        else stateMachine.ChangeState(stateMachine.idleState);

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
