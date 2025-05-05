using UnityEngine;

public class TEST_ThrowCoguState : TEST_CoguState
{
    private Vector3 _interactSpot;

    // Inerited Constructor
    public TEST_ThrowCoguState(TEST_CoguStateMachine stateMachine) : base(stateMachine) { }

    // Public Methods
    public TEST_ThrowCoguState Setup(Vector3 interactSpot)
    {
        this._interactSpot = interactSpot;
        return this;
    }

    // Inherited Public Methods
    public override void Enter()
    {
        Debug.Log("Enter - Throw");
    }

    public override void Update()
    {
        // Throw actions/animation

        _stateMachine.ChangeState(_stateMachine.MoveState.Setup(_interactSpot));
    }
}
