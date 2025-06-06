using UnityEngine;

public class ThrowCoguState : CoguState
{
    // Inerited Constructor
    public ThrowCoguState(CoguStateMachine stateMachine) : base(stateMachine) { }

    // Inherited Public Methods
    public override void Enter()
    {
        AudioManager.Instance.PlaySFX("CoguThrow");
    }

    public override void Update()
    {
        // Throw actions/animation

        _stateMachine.ChangeState(_stateMachine.MoveState);
    }
}
