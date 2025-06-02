using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class CoguCastter : MonoBehaviour, IResetable
{
    private PlayerInputActions _inputActions;

    // Fields
    [SerializeField] private CoguCastPoint _castPoint;
    [SerializeField] private float _interactRadius;
    [SerializeField, Range(0f, 1f)] private float _fieldOfView;
    [SerializeField] private LayerMask _interactableLayer;

    public int _coguCount;
    private int _coguHoldedAtCheckpoint;
    private bool _isAbleCast = true;

    // Properties
    public CoguCastPoint CastPoint { get { return _castPoint; } }
    public int CoguCount {  get { return _coguCount; } set { _coguCount = value; } }
    public bool IsAbleCast { get { return _isAbleCast;} set { _isAbleCast = value; } }

    private void OnEnable()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.Player.Enable();

        _inputActions.Player.SendCogu.started += SendCogu;
    }

    private void OnDisable()
    {
        // Input
        _inputActions.Player.SendCogu.started -= SendCogu;

        _inputActions.Player.Disable();

        // Reset
        RespawnController.OnPlayerChangeCheckPoint -= SaveResetState;
        RespawnController.Instance.TurnNonResetable(this);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Public Methods
    public void SendCogu(CallbackContext callbackContext)
    {
        if (_coguCount <= 0 || !_isAbleCast)
            return;

        _isAbleCast = false;

        Collider[] colliders = Physics.OverlapSphere(transform.position, _interactRadius, _interactableLayer, QueryTriggerInteraction.Collide);
        foreach (Collider obj in colliders)
        {
            if (obj.TryGetComponent(out CoguInteractable interactable))
            {
                Vector3 interactableDir = new Vector3(transform.position.x - interactable.transform.position.x, 0, transform.position.z - interactable.transform.position.z);
                Vector3 fowardDir = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z);
                if (-Vector3.Dot(fowardDir, interactableDir.normalized) < 1f - _fieldOfView)
                    continue;

                CastCogu(interactable.AssignedCoguType, interactable);
                return;
            }
        }

        _isAbleCast = true;
    }

    public void CastCogu(CoguType type, CoguInteractable interactable)
    {
        if (!interactable.IsAvailable)
            return;

        if (CoguManager.instance.TryGetCoguVariant(type, out Cogu variant))
        {
            Cogu cogu = Instantiate(variant.gameObject, _castPoint.transform.position, Quaternion.identity).GetComponent<Cogu>();
            cogu.Initialize(interactable, this);
            _coguCount--;
            _isAbleCast = false;
        }
    }

    #region // IResetable

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
