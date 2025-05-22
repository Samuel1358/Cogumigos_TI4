using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public class InteractingArea : ResetableBase
{
    [SerializeField] private InputActionAsset _inputActions;
    private InputAction _interact;

    [SerializeField] private Interaction _interaction;
    [SerializeField] private GameObject _visualInfo;

    private Collider _collider;
    private Player _player;
    private bool _isInteracted = false;

    protected override void OnEnable()
    {
        base.OnEnable();

        _inputActions.FindActionMap("Player").Enable();

        _interact = _inputActions.FindAction("Interact");       
    }

    private void OnDisable()
    {       
        _inputActions.FindActionMap("Player").Disable();
    }

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;

        if (_visualInfo != null)
            _visualInfo.SetActive(false);
    }

    // Public Methods
    public void Assign(Interaction interaction)
    {
        this._interaction = interaction;
    }

    // Private Methods
    private void InteractAction(InputAction.CallbackContext context)
    {
        if (_player != null)
        {
            _interaction.Interact(_player);
            _isInteracted = true;
            _interact.started -= InteractAction;

            NeedReset = true;
        }       
    }

    // Ressetable
    public override void ResetObject()
    {
        if (NeedReset)
        {
            _isInteracted = false;

            NeedReset = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((_interaction.InteractJustOnce) ? !_isInteracted : true)
        {
            // visual
            if (_visualInfo != null)
                _visualInfo.SetActive(true);

            Player player = other.GetComponentInParent<Player>();
            if (player == null)
                return;

            _interact.started += InteractAction;
            _player = player;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((_interaction.InteractJustOnce) ? !_isInteracted : true)
        {
            // visual
            if (_visualInfo != null)
                _visualInfo.SetActive(false);

            _interact.started -= InteractAction;
            _player = null;
        }
    }
}
