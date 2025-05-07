using UnityEngine;

public class AttractWildCoguState : WildCoguState
{
    private CoguCastPoint _assingPoint;

    // Inerited Constructor
    public AttractWildCoguState(WildCoguStateMachine stateMachine) : base(stateMachine) { }

    public override void Reset()
    {
        _assingPoint = null;
    }

    // Public Methods
    public AttractWildCoguState Setup(CoguCastPoint assingPoint)
    {
        this._assingPoint = assingPoint;
        _stateMachine.WildCogu.Agent.speed = _stateMachine.WildCogu.Data.moveSpd;
        return this;
    }

    // Inherited Public Methods
    public override void Enter()
    {
        //Debug.Log("Enter - Attract");
    }

    public override void Update()
    {
        _stateMachine.WildCogu.Agent.SetDestination(_assingPoint.transform.position);

        if (_stateMachine.WildCogu.ArrivedDestination())
        {
            _stateMachine.ChangeState(_stateMachine.DisappearState.Setup(_assingPoint));
        }
    }

    
}
