using UnityEngine;

public class IdleWildCoguState : WildCoguState
{
    private float _timer = 5f;

    // Inerited Constructor
    public IdleWildCoguState(WildCoguStateMachine stateMachine) : base(stateMachine) { }

    // Inherited Public Methods
    public override void Enter()
    {
        TweenHandler.Timer(_timer, RandomIdelAnimaion);
    }

    private void RandomIdelAnimaion()
    {
        _stateMachine.WildCogu.Animator.SetInteger("RandIdle", Random.Range(0, 3));
        TweenHandler.Timer(_timer, RandomIdelAnimaion);
    }
}
