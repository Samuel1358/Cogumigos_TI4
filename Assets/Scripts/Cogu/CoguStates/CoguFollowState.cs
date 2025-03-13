using UnityEngine;

public class CoguFollowState : CoguState
{
    // Constructor
    public CoguFollowState(CoguStateMachine stateMachine) : base(stateMachine) { }

    // Interface Methods
    public override void Enter() 
    {
        stateMachine.cogu.GetAgent().enabled = true;
        stateMachine.cogu.GetAgent().stoppingDistance = 1.5f;
        stateMachine.cogu.GetAgent().speed = stateMachine.cogu.GetFollowSpd();
    }

    public override void Exit() { }

    public override void HandleInput() { }

    public override void Update() 
    {
        if (stateMachine.cogu.GetTarget() != null)
        {
            stateMachine.cogu.GetAgent().SetDestination(stateMachine.cogu.Avoidence(stateMachine.cogu.GetTargetFollow().position));
        }
    }

    public override void PhysicsUpdate() { }

    public override void OnAnimationEnterEvent() { }

    public override void OnAnimationExitEvent() { }

    public override void OnAnimationTransitionEvent() { }

    public override void OnTriggerEnter(Collider collider) { }

    public override void OnTriggerExit(Collider collider) { }
}
