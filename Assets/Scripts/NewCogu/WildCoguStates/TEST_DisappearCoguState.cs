using UnityEngine;

public class TEST_DisappearWildCoguState : TEST_WildCoguState
{
    private TEST_CoguCastPoint _assingPoint;

    // Inerited Constructor
    public TEST_DisappearWildCoguState(TEST_WildCoguStateMachine stateMachine) : base(stateMachine) { }

    // Public Methods
    public TEST_DisappearWildCoguState Setup(TEST_CoguCastPoint assingPoint)
    {
        this._assingPoint = assingPoint;
        return this;
    }

    // Inherited Public Methods
    public override void Enter()
    {
        Debug.Log("Enter - Disappear");
    }

    public override void Update()
    {
        _assingPoint.AssingWildCogu(_stateMachine.WildCogu);
    }
}
