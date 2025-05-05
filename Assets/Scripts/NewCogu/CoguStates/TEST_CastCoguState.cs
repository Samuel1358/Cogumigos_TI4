using UnityEngine;

public class TEST_CastCoguState : TEST_CoguState
{
    private Vector3 _interactSpot;

    // Inerited Constructor
    public TEST_CastCoguState(TEST_CoguStateMachine stateMachine) : base(stateMachine) { }

    // Public Methods
    public TEST_CastCoguState Setup(Vector3 interactSpot)
    {
        this._interactSpot = interactSpot;
        return this;
    }

    // Inherited Public Methods
    public override void Enter()
    {
        Debug.Log("Enter - Cast");
    }

    public override void Update()
    {
        // Cast actions/animation

        _stateMachine.ChangeState(_stateMachine.ThrowState.Setup(_interactSpot));
    }
}
