using UnityEngine;

public class TEST_AttractWildCoguState : TEST_WildCoguState
{
    private TEST_CoguCastPoint _assingPoint;

    // Inerited Constructor
    public TEST_AttractWildCoguState(TEST_WildCoguStateMachine stateMachine) : base(stateMachine) { }

    // Public Methods
    public TEST_AttractWildCoguState Setup(TEST_CoguCastPoint assingPoint)
    {
        this._assingPoint = assingPoint;
        _stateMachine.WildCogu.Agent.speed = _stateMachine.WildCogu.Data.attractSpd;
        return this;
    }

    // Inherited Public Methods
    public override void Enter()
    {
        Debug.Log("Enter - Attract");
    }

    public override void Update()
    {
        _stateMachine.WildCogu.Agent.SetDestination(_assingPoint.transform.position);

        if (ArrivedDestination())
        {
            _stateMachine.ChangeState(_stateMachine.DisappearState.Setup(_assingPoint));
        }
    }

    // Private Methods
    private bool ArrivedDestination()
    {
        if (Vector3.Distance(_stateMachine.WildCogu.transform.position, _assingPoint.transform.position) <= _stateMachine.WildCogu.Data.minDistance)
            return true;
        else
            return false;
    }
}
