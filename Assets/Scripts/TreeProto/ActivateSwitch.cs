using UnityEngine;
using UnityEngine.Events;

public class ActivateSwitch : CoguInteractable
{
    [Header("Switch Settings")]
    [SerializeField] private bool _startActivated = false;
    [SerializeField] private Light _indicatorLight; // Optional visual indicator
    
    [Header("Interaction Settings")]
    [SerializeField] private bool _interactJustOnce = false;
    [SerializeField] private bool _toggleMode = true; // true = toggle, false = only activate
    [SerializeField] private KeyCode _interactionKey = KeyCode.E; // Key for direct interaction
    
    [Header("Player Detection")]
    [SerializeField] private float _detectionRadius = 3f;
    [SerializeField] private LayerMask _playerLayer = 1; // Default layer, adjust as needed
    [SerializeField] private Transform _detectionOrigin; // Optional custom origin for sphere cast
    
    [Header("State")]
    public bool isActivated = false;
    
    [Header("Events")]
    public UnityEvent onActivate;
    public UnityEvent onDeactivate;
    
    private ActivateSwitchInteraction _interaction;
    
    // Reset variables
    private bool _initialActivatedState;
    private bool _checkpointSavedState;
    
    // Player detection variables
    private bool _playerInRange = false;
    private bool _hasBeenActivated = false; // Track if switch was ever activated

    protected override void OnEnable()
    {
        base.OnEnable();
        // Subscribe to checkpoint changes
        GameIniciator.Instance.RespawnControllerInstance.OnPlayerChangeCheckPoint += SaveStateAtCheckpoint;
    }

    private void OnDisable()
    {
        // Unsubscribe from checkpoint changes
        GameIniciator.Instance.RespawnControllerInstance.OnPlayerChangeCheckPoint -= SaveStateAtCheckpoint;
    }

    /*private void Awake()
    {
        _interaction = ScriptableObject.CreateInstance<ActivateSwitchInteraction>();
        _interaction.Assign(this, HandleSwitchInteraction);

        // Configure if interaction should happen just once
        _interaction.SetInteractJustOnce(_interactJustOnce);

        _area.Assign(_interaction);
    }*/

    private void Start()
    {
        // Store initial state for reset
        _initialActivatedState = _startActivated;
        _checkpointSavedState = _startActivated;
        
        // Set initial state
        isActivated = _startActivated;
        UpdateVisuals();
        
        // If starting activated, mark as activated
        if (_startActivated)
        {
            _hasBeenActivated = true;
        }
    }

    private void Update()
    {
        CheckPlayerInRange();
        UpdateInteractableEffect();
        
        // Handle direct interaction input (only if no Cogu type is assigned)
        if (UsesDirectInteraction() && _playerInRange && Input.GetKeyDown(_interactionKey))
        {
            HandleDirectInteraction();
        }
    }

    private bool UsesDirectInteraction()
    {
        // Use direct interaction if no Cogu type is assigned (None)
        return AssignedCoguType == CoguType.None;
    }

    private bool UsesCoguInteraction()
    {
        // Use Cogu interaction if a Cogu type is assigned
        return AssignedCoguType != CoguType.None;
    }

    private void HandleDirectInteraction()
    {
        // Check if we should allow interaction based on activation state
        if (_interactJustOnce && _hasBeenActivated)
        {
            Debug.Log($"ActivateSwitch {name}: Direct interaction blocked - already activated once");
            return; // Don't allow interaction if it's one-time and already activated
        }
        
        Debug.Log($"ActivateSwitch {name}: Direct interaction triggered");
        HandleSwitchInteraction(this);
    }

    private void CheckPlayerInRange()
    {
        Vector3 origin = _detectionOrigin != null ? _detectionOrigin.position : transform.position;
        
        // Use Physics.OverlapSphere for better performance than sphere cast
        Collider[] colliders = Physics.OverlapSphere(origin, _detectionRadius, _playerLayer);
        
        bool wasInRange = _playerInRange;
        _playerInRange = colliders.Length > 0;
        
        // Debug logs for player detection when using direct interaction
        if (UsesDirectInteraction())
        {
            if (_playerInRange && !wasInRange)
            {
                Debug.Log($"ActivateSwitch {name}: Player entered range (Direct interaction mode)");
            }
            else if (!_playerInRange && wasInRange)
            {
                Debug.Log($"ActivateSwitch {name}: Player left range (Direct interaction mode)");
            }
        }
    }

    private void UpdateInteractableEffect()
    {
        if (_interactableEffectVisual != null)
        {
            // Show effect only if:
            // 1. Player is in range
            // 2. Switch hasn't been activated OR we're in toggle mode
            // 3. If it's been activated and we're not in toggle mode, don't show
            bool shouldShow = _playerInRange && 
                            (!_hasBeenActivated || _toggleMode);
            
            _interactableEffectVisual.SetActive(shouldShow);
        }
    }

    // CoguInteract
    public override void Interact(Cogu cogu)
    {
        // Only allow Cogu interaction if a Cogu type is assigned
        if (UsesCoguInteraction())
        {
            Debug.Log($"ActivateSwitch {name}: Cogu interaction with type {AssignedCoguType}");
            HandleSwitchInteraction(this);
            Destroy(cogu.gameObject);
        }
        else
        {
            Debug.Log($"ActivateSwitch {name}: Cogu interaction not allowed - no Cogu type assigned");
        }
    }

    // Public Methods
    public void Activate()
    {
        if (!isActivated)
        {
            isActivated = true;
            _hasBeenActivated = true; // Mark as activated
            UpdateVisuals();
            onActivate?.Invoke();
            Debug.Log($"ActivateSwitch {name} activated");
            
            // Mark that this object needs reset
            NeedReset = true;
        }
    }

    public void Deactivate()
    {
        if (isActivated)
        {
            isActivated = false;
            UpdateVisuals();
            onDeactivate?.Invoke();
            Debug.Log($"ActivateSwitch {name} deactivated");
            
            // Mark that this object needs reset
            NeedReset = true;
        }
    }

    public void Toggle()
    {
        if (isActivated)
            Deactivate();
        else
            Activate();
    }

    // Checkpoint state management
    private void SaveStateAtCheckpoint(Checkpoint checkpoint)
    {
        _checkpointSavedState = isActivated;
        Debug.Log($"ActivateSwitch {name}: Saved state at checkpoint: {_checkpointSavedState}");
    }

    // Resetable Implementation
    public override void ResetObject()
    {
        if (NeedReset)
        {
            base.ResetObject();

            // Determine what state to reset to based on checkpoint system
            bool targetState = _checkpointSavedState;
            
            Debug.Log($"Resetting ActivateSwitch {name} to checkpoint state: {targetState}");
            
            // Reset to checkpoint saved state
            isActivated = targetState;
            
            // Reset activation tracking based on checkpoint state
            _hasBeenActivated = targetState;
            
            UpdateVisuals();
            
            // Fire appropriate events to notify listeners
            if (isActivated)
                onActivate?.Invoke();
            else
                onDeactivate?.Invoke();
            
            NeedReset = false;
        }
    }

    // Method to force synchronization (useful for when listeners need to sync)
    public void ForceSynchronization()
    {
        if (isActivated)
            onActivate?.Invoke();
        else
            onDeactivate?.Invoke();
    }

    // Private Methods
    private void HandleSwitchInteraction(ActivateSwitch activateSwitch)
    {
        if (_toggleMode)
        {
            // Toggle mode: switch between activated/deactivated
            Toggle();
        }
        else
        {
            // Only activate mode: always activate when interacted
            Activate();
        }
    }

    private void UpdateVisuals()
    {
        if (_indicatorLight != null)
        {
            _indicatorLight.enabled = isActivated;
        }
    }
    
    // Gizmos for debugging
    private void OnDrawGizmosSelected()
    {
        Vector3 origin = _detectionOrigin != null ? _detectionOrigin.position : transform.position;
        Gizmos.color = _playerInRange ? Color.green : Color.red;
        Gizmos.DrawWireSphere(origin, _detectionRadius);
    }
} 