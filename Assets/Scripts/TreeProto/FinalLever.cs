using UnityEngine;

public class FinalLever : MonoBehaviour
{
    [Header("Light Settings")]
    public Light leverLight;
    public Color activeColor = Color.green;
    private Color originalColor;
    
    [Header("Switch Connection")]
    [SerializeField] private ActivateSwitch _activateSwitch;
    
    [Header("State")]
    public bool isActivated = false;

    private void OnEnable()
    {
        // Subscribe to respawn events
        GameIniciator.Instance.RespawnControllerInstance.OnPlayerRespawn += OnPlayerRespawn;
    }

    private void OnDisable()
    {
        // Unsubscribe from respawn events
        GameIniciator.Instance.RespawnControllerInstance.OnPlayerRespawn -= OnPlayerRespawn;
    }

    private void Start()
    {
        // Se não foi atribuída uma luz, tenta encontrar uma no objeto
        if (leverLight == null)
        {
            leverLight = GetComponentInChildren<Light>();
        }

        // Guarda a cor original da luz
        if (leverLight != null)
        {
            originalColor = leverLight.color;
        }
        
        // Subscribe to switch state changes
        if (_activateSwitch != null)
        {
            _activateSwitch.onActivate.AddListener(OnSwitchActivated);
            _activateSwitch.onDeactivate.AddListener(OnSwitchDeactivated);
            
            // Sync initial state with switch
            SyncWithSwitch();
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        if (_activateSwitch != null)
        {
            _activateSwitch.onActivate.RemoveListener(OnSwitchActivated);
            _activateSwitch.onDeactivate.RemoveListener(OnSwitchDeactivated);
        }
    }

    // Handle player respawn
    private void OnPlayerRespawn()
    {
        // Delay synchronization to ensure ActivateSwitch has been reset first
        Invoke(nameof(DelayedSyncWithSwitch), 0.1f);
    }

    private void DelayedSyncWithSwitch()
    {
        if (_activateSwitch != null)
        {
            SyncWithSwitch();
            Debug.Log($"FinalLever {name}: Re-synchronized with switch after respawn. Switch state: {_activateSwitch.isActivated}");
        }
    }

    // Private Methods
    private void OnSwitchActivated()
    {
        Activate();
    }

    private void OnSwitchDeactivated()
    {
        Deactivate();
    }

    private void SyncWithSwitch()
    {
        if (_activateSwitch == null) return;
        
        if (_activateSwitch.isActivated)
        {
            Activate();
        }
        else
        {
            Deactivate();
        }
    }

    public virtual void Activate()
    {
        // Se já estiver ativado, não faz nada
        if (isActivated) return;

        isActivated = true;
        
        // Liga a luz
        if (leverLight != null)
        {
            leverLight.enabled = true;
            leverLight.color = activeColor;
        }
        
        Debug.Log($"FinalLever {name} activated by switch");
    }

    public virtual void Deactivate()
    {
        if (!isActivated) return;

        isActivated = false;
        
        // Desliga a luz e restaura cor original
        if (leverLight != null)
        {
            leverLight.enabled = false;
            leverLight.color = originalColor;
        }
        
        Debug.Log($"FinalLever {name} deactivated by switch");
    }
} 