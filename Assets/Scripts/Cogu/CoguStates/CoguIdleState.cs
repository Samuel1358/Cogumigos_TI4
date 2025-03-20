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

    //public override void Exit() { }  

    //public override void HandleInput() { }

    public override void Update() 
    {
        base.Update();

        stateMachine.cogu.Chillin();
    }

    //public override void PhysicsUpdate() { }

    //public override void OnAnimationEnterEvent() { }

    //public override void OnAnimationExitEvent() { }

    //public override void OnAnimationTransitionEvent() { }

    //public override void OnTriggerEnter(Collider collider) { }

    //public override void OnTriggerExit(Collider collider) { }
}
