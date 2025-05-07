using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class WildCoguStateMachine : StateMachine
{
    #region // Fields

    private WildCogu _wildCogu;
    
    // States
    private IdleWildCoguState _idleState;
    private AttractWildCoguState _attracState;
    private DisappearWildCoguState _disappearState;

    #endregion

    #region // Preperties

    public WildCogu WildCogu { get { return _wildCogu; } }

    // States
    public IdleWildCoguState IdleState {  get { return _idleState; } }
    public AttractWildCoguState AttracState {  get { return _attracState; } }
    public DisappearWildCoguState DisappearState {  get { return _disappearState; } }

    #endregion

    // Constructor
    public WildCoguStateMachine(WildCogu wildCogu)
    {
        this._wildCogu = wildCogu;
        this._idleState = new IdleWildCoguState(this);
        this._attracState = new AttractWildCoguState(this);
        this._disappearState = new DisappearWildCoguState(this);
    }

    // Public Methods
    public void Reset()
    {
        IdleState.Reset();
        AttracState.Reset();
        DisappearState.Reset();

        ChangeState(IdleState);
    }
}
