using UnityEngine;

public class DisappearWildCoguState : WildCoguState
{
    private CoguCastPoint _assingPoint;

    // Inerited Constructor
    public DisappearWildCoguState(WildCoguStateMachine stateMachine) : base(stateMachine) { }

    // Public Methods
    public DisappearWildCoguState Setup(CoguCastPoint assingPoint)
    {
        this._assingPoint = assingPoint;
        return this;
    }

    // Inherited Public Methods
    public override void Enter()
    {
        //Debug.Log("Enter - Disappear");
    }

    public override void Update()
    {
        _assingPoint.AssingWildCogu(_stateMachine.WildCogu);
    }
}
