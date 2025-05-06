using UnityEngine;

public class CastCoguState : CoguState
{
    private Vector3 _interactSpot;

    // Inerited Constructor
    public CastCoguState(CoguStateMachine stateMachine) : base(stateMachine) { }

    // Public Methods
    public CastCoguState Setup(Vector3 interactSpot)
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
