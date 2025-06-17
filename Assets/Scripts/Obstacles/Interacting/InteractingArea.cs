using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(Collider))]
public class InteractingArea : ResetableBase
{
    private PlayerInputActions _inputActions;

    [SerializeField] private Interaction _interaction;
    [SerializeField] private GameObject _visualInfo;
    [SerializeField] private float _visualOffset = 1.5f; // Altura do texto acima do objeto

    private Collider _collider;
    private Player _player;
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
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;

        if (_visualInfo != null)
        {
            _visualInfo.SetActive(false);
            // Posiciona o visual acima do objeto
            _visualInfo.transform.position = transform.position + Vector3.up * _visualOffset;

        }
    }

    // Public Methods
    public void Assign(Interaction interaction)
    {
        this._interaction = interaction;
    }

    // Private Methods
    private void InteractAction(CallbackContext callbackContext)
    {
        if (_player != null)
        {
            _interaction.Interact(_player);
            _isInteracted = true;
            _inputActions.Player.Interact.started -= InteractAction;

            // visual
            if (_visualInfo != null)
                _visualInfo.SetActive(false);

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
            Player player = other.GetComponentInParent<Player>();
            if (player == null)
                return;

            // visual - só mostra se ainda não foi interagido
            if (_visualInfo != null && !_isInteracted)
                _visualInfo.SetActive(true);

            _inputActions.Player.Interact.started += InteractAction;
            _player = player;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((_interaction.InteractJustOnce) ? !_isInteracted : true)
        {
            // visual - sempre oculta quando sai da área
            if (_visualInfo != null)
                _visualInfo.SetActive(false);

            _inputActions.Player.Interact.started -= InteractAction;
            _player = null;
        }
    }
}

// Classe auxiliar para fazer o objeto sempre olhar para a câmera
