using UnityEngine;

public class ThrowCoguState : CoguState
{
    private Vector3 _interactSpot;

    // Inerited Constructor
    public ThrowCoguState(CoguStateMachine stateMachine) : base(stateMachine) { }

    // Public Methods
    public ThrowCoguState Setup(Vector3 interactSpot)
    {
        this._interactSpot = interactSpot;
        return this;
    }

    // Inherited Public Methods
    public override void Enter()
    {
        //Debug.Log("Enter - Throw");
    }

    public override void Update()
    {
        // Throw actions/animation

        _stateMachine.ChangeState(_stateMachine.MoveState.Setup(_interactSpot));
    }
}
