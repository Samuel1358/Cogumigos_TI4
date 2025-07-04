using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class ActivableObject : ResetableBase
{
    [Header("Required Switches")]
    public List<ActivateSwitch> requiredSwitches;

    [Header("Activation Settings")]
    public bool isActivated = false;
    public bool startOff = true; // Se true, o objeto começa desativado e aparece quando ativado
    public Light objectLight;
    public GameObject[] objectsToActivate;
    public GameObject[] objectsToDeactivate;

    [Header("Events")]
    public UnityEvent onActivate;
    public UnityEvent onDeactivate;

    private Renderer[] renderers;
    
    // Reset variables
    private bool _initialActivatedState;
    private bool _initialObjectActiveState;
    private bool _initialLightState;
    private List<bool> _initialObjectsToActivateStates;
    private List<bool> _initialObjectsToDeactivateStates;

    protected override void OnEnable()
    {
        base.OnEnable();
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
        if (objectLight == null)
        {
            objectLight = GetComponentInChildren<Light>();
        }

        // Pega todos os renderers do objeto e seus filhos
        renderers = GetComponentsInChildren<Renderer>();

        // Store initial states for reset
        StoreInitialStates();

        // Inscreve para eventos de todos os switches requeridos
        foreach (ActivateSwitch activateSwitch in requiredSwitches)
        {
            if (activateSwitch != null)
            {
                activateSwitch.onActivate.AddListener(CheckActivation);
                activateSwitch.onDeactivate.AddListener(CheckActivation);
            }
        }

        // Configura o estado inicial
        SetInitialState();
    }

    private void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        foreach (ActivateSwitch activateSwitch in requiredSwitches)
        {
            if (activateSwitch != null)
            {
                activateSwitch.onActivate.RemoveListener(CheckActivation);
                activateSwitch.onDeactivate.RemoveListener(CheckActivation);
            }
        }
    }

    // Handle player respawn
    private void OnPlayerRespawn()
    {
        // Delay synchronization to ensure ActivateSwitches have been reset first
        Invoke(nameof(DelayedCheckActivation), 0.2f);
    }

    private void DelayedCheckActivation()
    {
        // Force check activation based on current switch states
        CheckActivationAndForceUpdate();
        Debug.Log($"ActivableObject {name}: Re-checked activation after respawn");
    }

    private void CheckActivationAndForceUpdate()
    {
        bool allActivated = true;
        foreach (ActivateSwitch activateSwitch in requiredSwitches)
        {
            if (activateSwitch == null || !activateSwitch.isActivated)
            {
                allActivated = false;
                break;
            }
        }

        // Force update to correct state regardless of current state
        if (allActivated)
        {
            ForceActivate();
        }
        else
        {
            ForceDeactivate();
        }
    }

    private void StoreInitialStates()
    {
        _initialActivatedState = isActivated;
        _initialObjectActiveState = gameObject.activeSelf;
        _initialLightState = objectLight != null ? objectLight.enabled : false;
        
        // Store initial states of objects to activate
        _initialObjectsToActivateStates = new List<bool>();
        foreach (GameObject obj in objectsToActivate)
        {
            _initialObjectsToActivateStates.Add(obj != null ? obj.activeSelf : false);
        }
        
        // Store initial states of objects to deactivate
        _initialObjectsToDeactivateStates = new List<bool>();
        foreach (GameObject obj in objectsToDeactivate)
        {
            _initialObjectsToDeactivateStates.Add(obj != null ? obj.activeSelf : false);
        }
    }

    private void SetInitialState()
    {
        // Se startOff for true, o objeto começa desativado
        if (startOff)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }

        // Configura o estado inicial dos outros objetos
        UpdateState(!startOff);
    }

    private void CheckActivation()
    {
        bool allActivated = true;
        foreach (ActivateSwitch activateSwitch in requiredSwitches)
        {
            if (activateSwitch == null || !activateSwitch.isActivated)
            {
                allActivated = false;
                break;
            }
        }

        if (allActivated && !isActivated)
        {
            Activate();
        }
        else if (!allActivated && isActivated)
        {
            Deactivate();
        }
    }

    private void Activate()
    {
        isActivated = true;

        // Se startOff for true, ativar significa mostrar o objeto
        // Se startOff for false, ativar significa esconder o objeto
        gameObject.SetActive(startOff);
        UpdateState(startOff);
        onActivate?.Invoke();
        
        Debug.Log($"ActivableObject {name} activated");
        
        // Mark that this object needs reset
        NeedReset = true;
    }

    private void Deactivate()
    {
        isActivated = false;

        // Se startOff for true, desativar significa esconder o objeto
        // Se startOff for false, desativar significa mostrar o objeto
        gameObject.SetActive(!startOff);
        UpdateState(!startOff);
        onDeactivate?.Invoke();
        
        Debug.Log($"ActivableObject {name} deactivated");
        
        // Mark that this object needs reset
        NeedReset = true;
    }

    // Force activation regardless of current state (used for respawn sync)
    private void ForceActivate()
    {
        bool wasActivated = isActivated;
        isActivated = true;

        // Se startOff for true, ativar significa mostrar o objeto
        // Se startOff for false, ativar significa esconder o objeto
        gameObject.SetActive(startOff);
        UpdateState(startOff);
        
        if (!wasActivated)
        {
            onActivate?.Invoke();
        }
        
        Debug.Log($"ActivableObject {name} force activated");
    }

    // Force deactivation regardless of current state (used for respawn sync)
    private void ForceDeactivate()
    {
        bool wasActivated = isActivated;
        isActivated = false;

        // Se startOff for true, desativar significa esconder o objeto
        // Se startOff for false, desativar significa mostrar o objeto
        gameObject.SetActive(!startOff);
        UpdateState(!startOff);
        
        if (wasActivated)
        {
            onDeactivate?.Invoke();
        }
        
        Debug.Log($"ActivableObject {name} force deactivated");
    }

    // Resetable Implementation
    public override void ResetObject()
    {
        if (NeedReset)
        {
            Debug.Log($"ActivableObject {name}: ResetObject called - will be overridden by DelayedCheckActivation");
            
            // Don't reset to initial state immediately
            // Let DelayedCheckActivation handle the correct state based on switches
            // This prevents the "old state" bug
            
            NeedReset = false;
        }
    }

    private void UpdateState(bool active)
    {
        // Ativa/desativa a luz
        if (objectLight != null)
        {
            objectLight.enabled = active;
        }

        // Ativa/desativa objetos
        foreach (GameObject obj in objectsToActivate)
        {
            if (obj != null)
            {
                obj.SetActive(active);
            }
        }

        // Desativa/ativa objetos (comportamento inverso)
        foreach (GameObject obj in objectsToDeactivate)
        {
            if (obj != null)
            {
                obj.SetActive(!active);
            }
        }
    }
} 