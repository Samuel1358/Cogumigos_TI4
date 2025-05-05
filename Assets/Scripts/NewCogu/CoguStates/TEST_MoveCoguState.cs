using UnityEngine;

public class TEST_MoveCoguState : TEST_CoguState
{
    // Inerited Constructor
    public TEST_MoveCoguState(TEST_CoguStateMachine stateMachine) : base(stateMachine) { }

    // Inherited Public Methods
    public override void Enter()
    {
        Debug.Log("Enter - Move");
    }
}
