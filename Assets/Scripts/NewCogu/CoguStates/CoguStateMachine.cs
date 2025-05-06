using UnityEngine;

public class CoguStateMachine : StateMachine
{
    
    #region // Fields

    private Cogu _cogu;

    // States
    private CastCoguState _castState;
    private ThrowCoguState _throwState;
    private MoveCoguState _moveState;
    private InteractCoguState _interactState;

    #endregion

    #region // Preperties

    public Cogu Cogu { get { return _cogu; } }

    // States
    public CastCoguState CastState { get { return _castState;} }
    public ThrowCoguState ThrowState { get { return _throwState;} }
    public MoveCoguState MoveState { get { return _moveState;} }
    public InteractCoguState InteractState { get { return _interactState; } }

    #endregion

    // Constructor
    public CoguStateMachine(Cogu cogu)
    {
        this._cogu = cogu;
        this._castState = new CastCoguState(this);
        this._throwState = new ThrowCoguState(this);
        this._moveState = new MoveCoguState(this);
        this._interactState = new InteractCoguState(this);
    }
    
}
