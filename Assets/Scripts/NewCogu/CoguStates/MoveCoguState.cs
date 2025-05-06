using UnityEngine;

public class MoveCoguState : CoguState
{
    // Inerited Constructor
    public MoveCoguState(CoguStateMachine stateMachine) : base(stateMachine) { }

    // Public Methods
    public MoveCoguState Setup(Vector3 interactSpot)
    {
        _stateMachine.Cogu.Agent.speed = _stateMachine.Cogu.Data.moveSpd;
        _stateMachine.Cogu.Agent.SetDestination(interactSpot);
        return this;
    }    

    // Inherited Public Methods
    public override void Enter()
    {
        Debug.Log("Enter - Move");
    }

    public override void Update()
    {
        if (_stateMachine.Cogu.ArrivedDestination())
            _stateMachine.ChangeState(_stateMachine.InteractState);
    } 
}
