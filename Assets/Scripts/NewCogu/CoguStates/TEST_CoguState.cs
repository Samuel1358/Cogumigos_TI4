using UnityEngine;

public class TEST_CoguState : IState
{
    // Fields
    protected TEST_CoguStateMachine _stateMachine;

    // Constructor
    public TEST_CoguState(TEST_CoguStateMachine stateMachine)
    {
        this._stateMachine = stateMachine;
    }

    // Interface public Methods
    public virtual void Enter() { }

    public virtual void Exit() { }

    public virtual void HandleInput() { }

    public virtual void Update() { }

    public virtual void PhysicsUpdate() { }

    public virtual void OnAnimationEnterEvent() { }

    public virtual void OnAnimationExitEvent() { }

    public virtual void OnAnimationTransitionEvent() { }

    public virtual void OnTriggerEnter(Collider collider) { }

    public virtual void OnTriggerExit(Collider collider) { }
}
