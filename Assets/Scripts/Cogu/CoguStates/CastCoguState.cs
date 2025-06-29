using UnityEngine;

public class CastCoguState : CoguState
{
    // Inerited Constructor
    public CastCoguState(CoguStateMachine stateMachine) : base(stateMachine) { }

    // Inherited Public Methods
    public override void Enter()
    {
        //Debug.Log("Enter - Cast");
        _stateMachine.Cogu.AnimToThrow();
    }

    public override void Update()
    {
        // Cast actions/animation

        
    }
}
