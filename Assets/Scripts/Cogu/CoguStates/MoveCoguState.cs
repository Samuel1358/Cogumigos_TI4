using UnityEngine;

public class MoveCoguState : CoguState
{
    private Vector3 _heightPoint;
    private float t = 0;
    private LayerMask _rayMask;

    // Inerited Constructor
    public MoveCoguState(CoguStateMachine stateMachine) : base(stateMachine) { }

    // Inherited Public Methods
    public override void Enter()
    {
        Vector3 heightPoint = Vector3.Lerp(_stateMachine.Cogu.InteractSpot, _stateMachine.Cogu.CastSpot, .5f);
        heightPoint.y += _stateMachine.Cogu.Data.throwHeight;
        this._heightPoint = heightPoint;

        _rayMask = LayerMask.GetMask("Ground", "Wall");
    }

    public override void Update()
    {
        Parable();
    }

    private void Parable()
    {
        if (t <= 1f)
        {
            Vector3 a = Vector3.Lerp(_stateMachine.Cogu.CastSpot, _heightPoint, t);
            Vector3 b = Vector3.Lerp(_heightPoint, _stateMachine.Cogu.InteractSpot, t);

            _stateMachine.Cogu.transform.Translate(Vector3.Lerp(a, b, t) - _stateMachine.Cogu.transform.position);

            t += Time.fixedDeltaTime;
        }
        else
        {
            _stateMachine.ChangeState(_stateMachine.InteractState);
        }
    }
}
