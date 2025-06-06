using UnityEngine;

public class CastCoguState : CoguState
{
    // Inerited Constructor
    public CastCoguState(CoguStateMachine stateMachine) : base(stateMachine) { }

    // Inherited Public Methods
    public override void Enter()
    {
        //Debug.Log("Enter - Cast");
    }

    public override void Update()
    {
        // Cast actions/animation

        _stateMachine.ChangeState(_stateMachine.ThrowState);
    }
}
