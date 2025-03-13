using UnityEngine;

public class CoguIdleState : CoguState
{
    // Constructor
    public CoguIdleState(CoguStateMachine stateMachine) : base(stateMachine) { }

    // Interface Methods
    public override void Enter() 
    {
        stateMachine.cogu.SetTarget(default);
        stateMachine.cogu.SetTargetFollow(null);
    }

    public override void Exit() { }  

    public override void HandleInput() { }

    public override void Update() 
    {
        stateMachine.cogu.Chillin();
    }

    public override void PhysicsUpdate() { }

    public override void OnAnimationEnterEvent() { }

    public override void OnAnimationExitEvent() { }

    public override void OnAnimationTransitionEvent() { }

    public override void OnTriggerEnter(Collider collider) { }

    public override void OnTriggerExit(Collider collider) { }
}
