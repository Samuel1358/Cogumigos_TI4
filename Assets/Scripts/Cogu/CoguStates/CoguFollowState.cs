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

    //public override void Exit() { }

    //public override void HandleInput() { }

    public override void Update() 
    {
        base.Update();

        stateMachine.cogu.Follow();
    }

    //public override void PhysicsUpdate() { }

    //public override void OnAnimationEnterEvent() { }

    //public override void OnAnimationExitEvent() { }

    //public override void OnAnimationTransitionEvent() { }

    //public override void OnTriggerEnter(Collider collider) { }

    //public override void OnTriggerExit(Collider collider) { }
}
