using System;
using UnityEngine;

public class CoguInteractingState : CoguState
{
    private event Action interacting;

    // Constructor
    public CoguInteractingState(CoguStateMachine stateMachine) : base(stateMachine, "Interacting") { }

    public override void Update()
    {
        base.Update();

        interacting?.Invoke();
    }

    public CoguState StartInteracting(Action act)
    {
        interacting += act;
        return this;
    }

    public void EndInteracting(Action act)
    {
        interacting -= act;
        stateMachine.ChangeState(stateMachine.attractState);
    }
}
