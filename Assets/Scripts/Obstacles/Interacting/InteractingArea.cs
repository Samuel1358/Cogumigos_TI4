using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(Collider))]
public class InteractingArea : ResetableBase
{
    private PlayerInputActions _inputActions;

    [SerializeField] private Interaction _interaction;
    [SerializeField] private GameObject _visualInfo;
    [SerializeField] private float _visualOffset = 1.5f; // Altura do texto acima do objeto
    [SerializeField] private bool _useAutoPositioning = false; // Se deve posicionar automaticamente ou usar a posição do prefab

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
            
            // Posiciona o visual apenas se a opção de posicionamento automático estiver ativada
            if (_useAutoPositioning)
            {
                _visualInfo.transform.position = transform.position + Vector3.up * _visualOffset;
            }
        }
    }

    // Public Methods
    public void Assign(Interaction interaction)
    {
        this._interaction = interaction;
    }

    /// <summary>
    /// Reposiciona o VisualInfo usando o offset configurado
    /// </summary>
    public void RepositionVisualInfo()
    {
        if (_visualInfo != null)
        {
            _visualInfo.transform.position = transform.position + Vector3.up * _visualOffset;
        }
    }

    /// <summary>
    /// Define a posição do VisualInfo manualmente
    /// </summary>
    public void SetVisualInfoPosition(Vector3 position)
    {
        if (_visualInfo != null)
        {
            _visualInfo.transform.position = position;
        }
    }

    /// <summary>
    /// Ativa/desativa o posicionamento automático
    /// </summary>
    public void SetAutoPositioning(bool useAuto)
    {
        _useAutoPositioning = useAuto;
        if (useAuto && _visualInfo != null)
        {
            RepositionVisualInfo();
        }
    }

    // Private Methods
    private void InteractAction(CallbackContext callbackContext)
    {
        if (_player != null)
        {
            _interaction.Interact(_player);
            _isInteracted = true;
            _inputActions.Player.Interact.started -= InteractAction;

            // visual - só desativa permanentemente se for interação única
            // para objetos de múltiplas interações, o feedback será gerenciado pelo OnTriggerExit
            if (_visualInfo != null && _interaction.InteractJustOnce)
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

            // visual - mostra sempre para objetos que podem ser interagidos múltiplas vezes
            // ou apenas se ainda não foi interagido para objetos de interação única
            if (_visualInfo != null && (_interaction.InteractJustOnce ? !_isInteracted : true))
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
