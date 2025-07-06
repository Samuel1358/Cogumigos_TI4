using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class CoguCastter : MonoBehaviour, IResetable
{
    private PlayerInputActions _inputActions;

    // Fields
    [SerializeField] private CoguCastPoint _castPoint;
    [SerializeField] private Player _player;
    [SerializeField] private float _interactRadius;
    [SerializeField, Range(0f, 1f)] private float _fieldOfView;
    [SerializeField] private LayerMask _interactableLayer;

    public int _coguCount;
    private int _coguHoldedAtCheckpoint;
    private bool _isAbleCast = true;

    private CoguType _coguType = CoguType.None;
    private CoguInteractable _coguInteractable = null;

    // Properties
    public CoguCastPoint CastPoint { get { return _castPoint; } }
    public int CoguCount {  get { return _coguCount; } set { _coguCount = value; } }
    public bool IsAbleCast { get { return _isAbleCast;} set { _isAbleCast = value; } }

    private void OnEnable()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.Player.Enable();

        _inputActions.Player.Interact.started += SendCogu;
    }

    private void OnDisable()
    {
        // Input
        _inputActions.Player.Interact.started -= SendCogu;

        _inputActions.Player.Disable();

        // Reset
        GameIniciator.Instance.RespawnControllerInstance.OnPlayerChangeCheckPoint -= SaveResetState;
        GameIniciator.Instance.RespawnControllerInstance.TurnNonResetable(this);
    }

    private void Teste(CallbackContext callbackContext)
    {
        Debug.Log("Teste 123");
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Public Methods
    public void SendCogu(CallbackContext callbackContext)
    {
        Debug.Log(01);
        if (_coguCount <= 0 || !_isAbleCast)
            return;

        _isAbleCast = false;

        Debug.Log(02);
        Collider[] colliders = Physics.OverlapSphere(transform.position, _interactRadius, _interactableLayer, QueryTriggerInteraction.Collide);
        foreach (Collider obj in colliders)
        {
            Debug.Log(03);
            if (obj.TryGetComponent(out CoguInteractable interactable))
            {
                Debug.Log(04);
                Vector3 interactableDir = new Vector3(Camera.main.transform.position.x - interactable.transform.position.x, 0, Camera.main.transform.position.z - interactable.transform.position.z);
                Vector3 fowardDir = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z);
                if (-Vector3.Dot(fowardDir, interactableDir.normalized) < 1f - _fieldOfView)
                    continue;

                if (interactable.AssignedCoguType == CoguType.None)
                    continue;

                if (interactable == null)
                    continue;

                if (!interactable.IsAvailable)
                    continue;

                Debug.Log("COGU CAST - send cogu");
                _coguType = interactable.AssignedCoguType;
                _coguInteractable = interactable;

                _player.PlayerAnimator.SetBool("IsThrowing", true);

                TweenHandler.Timer(1f, () => { _isAbleCast = true; Debug.Log("COGU CAST - tween timer"); });
                return;
            }
        }

        _isAbleCast = true;
    }

    public void CastCogu()
    {
        Debug.Log("COGU CAST - public cogu cast");
        CastCogu(_coguType, _coguInteractable);

        _coguType = CoguType.None;
        _coguInteractable = null;
    }

    // Private Methods
    private void CastCogu(CoguType type, CoguInteractable interactable)
    {
        if (type == CoguType.None)
        {
            Debug.Log("COGU CAST - type none");
            _isAbleCast = true;
            return;
        }

        if (interactable == null)
        {
            Debug.Log("COGU CAST - interactable null");
            _isAbleCast = true;
            return;
        }

        if (!interactable.IsAvailable)
        {
            Debug.Log("COGU CAST - interactable not available");
            Debug.Log(interactable.IsAvailable);
            _isAbleCast = true;
            return;
        }

        Debug.Log("COGU CAST - private cogu cast");
        if (GameIniciator.Instance.CoguManagerInstance.TryGetCoguVariant(type, out Cogu variant))
        {
            Debug.Log("COGU CAST - cast");
            Cogu cogu = Instantiate(variant.gameObject, _castPoint.transform.position, Quaternion.identity).GetComponent<Cogu>();
            cogu.Initialize(interactable, this);
            _coguCount--;
            GameIniciator.Instance.CanvasIniciatorInstance.InventoryCanvas.UpdateCoguCountUI(_coguCount);
            _isAbleCast = false;
        }
    }

    #region // IResetable

    // Public Methods
    public void Initialize() 
    {
        GameIniciator.Instance.RespawnControllerInstance.OnPlayerChangeCheckPoint += SaveResetState;
        GameIniciator.Instance.RespawnControllerInstance.TurnResetable(this);
    }

    public void SaveResetState(Checkpoint checkpoint)
    {
        _coguHoldedAtCheckpoint = _coguCount;
    }

    // Interface Public Methods
    public void ResetObject()
    {
        _coguCount = _coguHoldedAtCheckpoint;
        _isAbleCast = true;
    }

    #endregion   
}
