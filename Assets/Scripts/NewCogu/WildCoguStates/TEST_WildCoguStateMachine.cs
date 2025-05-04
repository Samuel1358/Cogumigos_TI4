using UnityEngine;

public class TEST_WildCoguStateMachine : StateMachine
{
    #region // Fields

    private TEST_WildCogu _wildCogu;
    
    // States
    private TEST_IdleWildCoguState _idleState;
    private TEST_AttractWildCoguState _attracState;
    private TEST_DisappearWildCoguState _disappearState;

    #endregion

    #region // Preperties

    public TEST_WildCogu WildCogu { get { return _wildCogu; } }

    // States
    public TEST_IdleWildCoguState IdleState {  get { return _idleState; } }
    public TEST_AttractWildCoguState AttracState {  get { return _attracState; } }
    public TEST_DisappearWildCoguState DisappearState {  get { return _disappearState; } }

    #endregion

    // Constructor
    public TEST_WildCoguStateMachine(TEST_WildCogu wildCogu)
    {
        this._wildCogu = wildCogu;
        this._idleState = new TEST_IdleWildCoguState(this);
        this._attracState = new TEST_AttractWildCoguState(this);
        this._disappearState = new TEST_DisappearWildCoguState(this);
    }
}
