using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(Collider))]
public class KeyedDoorInteract : ResetableBase
{
    private PlayerInputActions _inputActions;

    [Header("Esternal Accesses")]
    [SerializeField] private Switchable _switchable;
    [SerializeField] private GameObject _visualInfo;

    [Header("Settings")]
    [SerializeField] private KeyTypes _keyAccepted;

    private bool _isInteracting = false;
    private bool _isInteracted = false;

    protected override void OnEnable()
    {
        base.OnEnable();

        _inputActions = new PlayerInputActions();
        _inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Player.Disable();
    }

    private void Start()
    {
        GetComponent<Collider>().isTrigger = true;
        if (_visualInfo != null)
            _visualInfo.SetActive(false);
    }

    // Private Methods
    private void Interact(CallbackContext callbackContext)
    {
        _isInteracting = true;
    }

    public override void ResetObject()
    {
        if (NeedReset)
        {
            _isInteracting = false;
            _isInteracted = false;

            NeedReset = false;
        }
    }

    // MonoBehaviour Methods
    private void OnTriggerEnter(Collider other)
    {
        if (!_isInteracted)
        {
            _inputActions.Player.Interact.started += Interact;

            // visual
            Player player = other.GetComponentInParent<Player>();
            if (player == null)
                return;

            if (_visualInfo != null && player.Inventory.VerifyKey(_keyAccepted))
                _visualInfo.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!_isInteracted && _isInteracting)
        {
            Player player = other.GetComponentInParent<Player>();
            if (player == null)
            {
                _isInteracting = false;
                return;
            }

            if (player.Inventory.TryUseKey(_keyAccepted))
            {
                _switchable.Activate();
                _isInteracted = true;

                // visual
                if (_visualInfo != null)
                    _visualInfo.SetActive(false);

                NeedReset = true;
            }
        }       
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_isInteracted)
        {
            _inputActions.Player.Interact.started -= Interact;

            // visual
            if (_visualInfo != null)
                _visualInfo.SetActive(false);
        }
    }
}
