using UnityEngine;

public class ThrowCoguState : CoguState
{
    // Inerited Constructor
    public ThrowCoguState(CoguStateMachine stateMachine) : base(stateMachine) { }

    // Inherited Public Methods
    public override void Enter()
    {
        GameIniciator.Instance.AudioManagerInstance.PlaySFX(SoundEffectNames.COGU_ARREMESSO);
    }

    public override void Update()
    {
        // Throw actions/animation

        _stateMachine.ChangeState(_stateMachine.MoveState);
    }
}
