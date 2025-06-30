using UnityEngine;
using UnityEngine.Events;

public class ActivateSwitch : CoguInteractable
{
    [Header("Switch Settings")]
    [SerializeField] private bool _startActivated = false;
    [SerializeField] private Light _indicatorLight; // Optional visual indicator
    
    [Header("Interaction Settings")]
    //[SerializeField] private InteractingArea _area;
    [SerializeField] private bool _interactJustOnce = false;
    [SerializeField] private bool _toggleMode = true; // true = toggle, false = only activate
    
    [Header("State")]
    public bool isActivated = false;
    
    [Header("Events")]
    public UnityEvent onActivate;
    public UnityEvent onDeactivate;
    
    private ActivateSwitchInteraction _interaction;
    
    // Reset variables
    private bool _initialActivatedState;
    private bool _checkpointSavedState;

    protected override void OnEnable()
    {
        base.OnEnable();
        // Subscribe to checkpoint changes
        RespawnController.OnPlayerChangeCheckPoint += SaveStateAtCheckpoint;
    }

    private void OnDisable()
    {
        // Unsubscribe from checkpoint changes
        RespawnController.OnPlayerChangeCheckPoint -= SaveStateAtCheckpoint;
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
    }

    // CoguInteract
    public override void Interact(Cogu cogu)
    {
        Debug.Log("Interactable Pira");
        HandleSwitchInteraction(this);
        Destroy(cogu.gameObject);
    }

    // Public Methods
    public void Activate()
    {
        if (!isActivated)
        {
            isActivated = true;
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
        base.ResetObject();

        if (NeedReset)
        {
            // Determine what state to reset to based on checkpoint system
            bool targetState = _checkpointSavedState;
            
            Debug.Log($"Resetting ActivateSwitch {name} to checkpoint state: {targetState}");
            
            // Reset to checkpoint saved state
            isActivated = targetState;
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
} 