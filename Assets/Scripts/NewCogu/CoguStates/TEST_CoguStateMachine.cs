using UnityEngine;

public class TEST_CoguStateMachine : StateMachine
{
    
    #region // Fields

    private TEST_Cogu _cogu;

    // States
    private TEST_CastCoguState _castState;
    private TEST_ThrowCoguState _throwState;
    private TEST_MoveCoguState _moveState;
    private TEST_InteractCoguState _interactState;

    #endregion

    #region // Preperties

    public TEST_Cogu Cogu { get { return _cogu; } }

    // States
    public TEST_CastCoguState CastState { get { return _castState;} }
    public TEST_ThrowCoguState ThrowState { get { return _throwState;} }
    public TEST_MoveCoguState MoveState { get { return _moveState;} }
    public TEST_InteractCoguState InteractState { get { return _interactState; } }

    #endregion

    // Constructor
    public TEST_CoguStateMachine(TEST_Cogu cogu)
    {
        this._cogu = cogu;
        this._castState = new TEST_CastCoguState(this);
        this._throwState = new TEST_ThrowCoguState(this);
        this._moveState = new TEST_MoveCoguState(this);
        this._interactState = new TEST_InteractCoguState(this);
    }
    
}
