using UnityEngine;

public class TEST_ThrowCoguState : TEST_CoguState
{
    // Inerited Constructor
    public TEST_ThrowCoguState(TEST_CoguStateMachine stateMachine) : base(stateMachine) { }

    // Inherited Public Methods
    public override void Enter()
    {
        Debug.Log("Enter - Throw");
    }
}
