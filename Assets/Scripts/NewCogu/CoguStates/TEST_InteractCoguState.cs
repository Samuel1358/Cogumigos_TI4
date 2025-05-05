using UnityEngine;

public class TEST_InteractCoguState : TEST_CoguState
{
    // Inerited Constructor
    public TEST_InteractCoguState(TEST_CoguStateMachine stateMachine) : base(stateMachine) { }

    // Inherited Public Methods
    public override void Enter()
    {
        Debug.Log("Enter - Interact");
    }
}
