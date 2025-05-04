using UnityEngine;

public class TEST_CoguCastter : MonoBehaviour, IResetable
{
    // Fields
    public int _coguCount;
    private int _coguHoldedAtCheckpoint;

    // Properties
    public int CoguCount {  get { return _coguCount; } set { _coguCount = value; } }

    // PLAYER.START() - mudar depois
    private void Start()
    {
        Initialize();
    }

    private void OnDisable() 
    {
        RespawnController.OnPlayerChangeCheckPoint -= SaveResetState;
        RespawnController.Instance.TurnNonResetable(this);
    }

    // Public Methods
    public void Initialize() 
    {
        RespawnController.OnPlayerChangeCheckPoint += SaveResetState;
        RespawnController.Instance.TurnResetable(this);
    }

    public void SaveResetState(Checkpoint checkpoint)
    {
        _coguHoldedAtCheckpoint = _coguCount;
    }

    // Interface Public Methods
    public void ResetObject()
    {
        _coguCount = _coguHoldedAtCheckpoint;
    }
}
