using UnityEngine;

public class CoguAttractState : CoguState
{
    // Constructor
    public CoguAttractState(CoguStateMachine statemachine) : base(statemachine, "Attract") { }

    // Interface Methods
    public override void Enter() 
    {
        base.Enter();

        stateMachine.cogu.SetTarget(default);
        stateMachine.cogu.SetTargetFollow(CoguArmy.instance.GetFollowTarget());

        stateMachine.cogu.GetAgent().enabled = true;
        stateMachine.cogu.GetAgent().speed = stateMachine.cogu.GetAttractSpd();
    }

    //public override void Exit() { }

    //public override void HandleInput() { }

    public override void Update() 
    {
        base.Update();

        if (stateMachine.cogu.GetTarget() != null)
        {
            stateMachine.cogu.GetAgent().SetDestination(stateMachine.cogu.Avoidence(stateMachine.cogu.GetTargetFollow().position));
        }

        CoguArmy.instance.UpdateArmy(stateMachine.cogu);
    }

    //public override void PhysicsUpdate() { }

    //public override void OnAnimationEnterEvent() { }

    //public override void OnAnimationExitEvent() { }

    //public override void OnAnimationTransitionEvent() { }

    //public override void OnTriggerEnter(Collider collider) { }

    //public override void OnTriggerExit(Collider collider) { }
}
