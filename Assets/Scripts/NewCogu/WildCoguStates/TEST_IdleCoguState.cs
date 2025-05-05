using UnityEngine;

public class TEST_IdleWildCoguState : TEST_WildCoguState
{
    // Inerited Constructor
    public TEST_IdleWildCoguState(TEST_WildCoguStateMachine stateMachine) : base(stateMachine) { }

    // Inherited Public Methods
    public override void Enter()
    {
        Debug.Log("Enter - Idle");
    }
}
