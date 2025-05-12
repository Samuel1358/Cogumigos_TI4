using UnityEngine;

public class CoguCastter : MonoBehaviour, IResetable
{
    // Fields
    [SerializeField] private CoguCastPoint _castPoint;

    public int _coguCount;
    private int _coguHoldedAtCheckpoint;
    private bool _isAbleCast = true;

    // Properties
    public int CoguCount {  get { return _coguCount; } set { _coguCount = value; } }
    public bool IsAbleCast { get { return _isAbleCast;} set { _isAbleCast = value; } }

    // Public Methods
    public void CastCogu(CoguType type, Vector3 interactSpot, CoguInteractable interactable)
    {
        if (_coguCount > 0 && _isAbleCast)
        {
            if(CoguManager.instance.TryGetCoguVariant(type, out Cogu variant))
            {
                Debug.Log(_castPoint);
                Cogu cogu = Instantiate(variant.gameObject, _castPoint.transform.position, Quaternion.identity).GetComponent<Cogu>();
                cogu.Initialize(interactSpot, interactable, this);
                _coguCount--;
                _isAbleCast = false;
                Debug.LogWarning("Casted");
            }          
        }
        else
        {
            Debug.LogWarning("You can't cast!");
        }
    }

    #region // IResetable
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

    #endregion
}
