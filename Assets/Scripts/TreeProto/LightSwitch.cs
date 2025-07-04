using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    [Header("Light Settings")]
    [SerializeField] private Light _light;
    [SerializeField] private bool _startLightOff = true;
    
    [Header("Switch Connection")]
    [SerializeField] private ActivateSwitch _activateSwitch;
    
    [Header("Effects")]
    [SerializeField] private GameObject _flameEffect;
    
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
        // Set initial light state
        if (_light != null)
        {
            _light.enabled = !_startLightOff;
        }
        
        // Set initial flame effect state
        if (_flameEffect != null)
        {
            _flameEffect.SetActive(!_startLightOff);
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
            Debug.Log($"LightSwitch {name}: Re-synchronized with switch after respawn. Switch state: {_activateSwitch.isActivated}");
        }
    }

    // Public Methods
    public Light GetLight()
    {
        return _light;
    }

    // Private Methods
    private void OnSwitchActivated()
    {
        if (_light != null)
        {
            _light.enabled = true;
            Debug.Log($"Light {_light.name} turned ON by switch");
            //GameIniciator.Instance.AudioManagerInstance.PlaySFX(SoundEffectNames.COGU_VELA);
        }
        
        if (_flameEffect != null)
        {
            _flameEffect.SetActive(true);
            Debug.Log($"FlameEffect {_flameEffect.name} activated by switch");
        }
    }

    private void OnSwitchDeactivated()
    {
        if (_light != null)
        {
            _light.enabled = false;
            Debug.Log($"Light {_light.name} turned OFF by switch");
        }
        
        if (_flameEffect != null)
        {
            _flameEffect.SetActive(false);
            Debug.Log($"FlameEffect {_flameEffect.name} deactivated by switch");
        }
    }

    private void SyncWithSwitch()
    {
        if (_activateSwitch == null) return;
        
        if (_activateSwitch.isActivated)
        {
            OnSwitchActivated();
        }
        else
        {
            OnSwitchDeactivated();
        }
    }
} 