using UnityEngine;

public class DisappearWildCoguState : WildCoguState
{
    private CoguCastPoint _assingPoint;

    // Inerited Constructor
    public DisappearWildCoguState(WildCoguStateMachine stateMachine) : base(stateMachine) { }

    public override void Reset()
    {
        _assingPoint = null;
    }

    // Public Methods
    public DisappearWildCoguState Setup(CoguCastPoint assingPoint)
    {
        this._assingPoint = assingPoint;
        return this;
    }

    // Inherited Public Methods
    public override void Enter()
    {       
        _assingPoint.AssingWildCogu(_stateMachine.WildCogu);
    }
}
