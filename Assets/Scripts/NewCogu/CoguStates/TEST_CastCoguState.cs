using UnityEngine;

public class TEST_CastCoguState : TEST_CoguState
{
    // Inerited Constructor
    public TEST_CastCoguState(TEST_CoguStateMachine stateMachine) : base(stateMachine) { }

    // Inherited Public Methods
    public override void Enter()
    {
        Debug.Log("Enter - Cast");
    }
}
