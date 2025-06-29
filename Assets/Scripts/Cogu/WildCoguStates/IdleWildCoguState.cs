using UnityEngine;

public class IdleWildCoguState : WildCoguState
{
    // Inerited Constructor
    public IdleWildCoguState(WildCoguStateMachine stateMachine) : base(stateMachine) { }

    // Inherited Public Methods
    public override void Update()
    {
        //Debug.Log("Enter - Idle");
    }
}
