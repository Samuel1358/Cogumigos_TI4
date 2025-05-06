using UnityEngine;

public class WildCoguState: IState
{
    // Fields
    protected WildCoguStateMachine _stateMachine;

    // Constructor
    public WildCoguState (WildCoguStateMachine stateMachine)
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
