using UnityEngine;
using UnityEngine.Events;

public class LightBeamTarget : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private bool _requiresContinuousHit = false; // Se true, precisa manter o feixe no alvo
    [SerializeField] private float _activationDelay = 0f; // Delay antes de ativar
    [SerializeField] private bool _oneTimeActivation = false; // Se true, só ativa uma vez
    
    [Header("Object to Disable")]
    [SerializeField] private GameObject _objectToDisable; // Objeto que será desabilitado quando o alvo for atingido
    [SerializeField] private bool _disableOnHit = true; // Se true, desabilita o objeto quando atingido
    
    [Header("Visual Feedback")]
    [SerializeField] private GameObject _hitEffect; // Efeito visual quando é atingido
    [SerializeField] private Light _indicatorLight; // Luz indicadora opcional
    [SerializeField] private Color _activeColor = Color.green;
    [SerializeField] private Color _inactiveColor = Color.red;
    [SerializeField] private Renderer _targetRenderer; // Renderer para mudar cor
    
    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _hitSound;
    [SerializeField] private AudioClip _activationSound;
    
    [Header("Events")]
    public UnityEvent onBeamHit; // Evento quando o feixe atinge o alvo
    public UnityEvent onBeamMiss; // Evento quando o feixe para de atingir o alvo
    public UnityEvent onTargetActivated; // Evento quando o alvo é ativado
    public UnityEvent onTargetDeactivated; // Evento quando o alvo é desativado
    
    [Header("State")]
    [SerializeField] private bool _isActivated = false;
    [SerializeField] private bool _isBeingHit = false;
    
    private bool _hasBeenActivatedOnce = false;
    private Coroutine _activationCoroutine;
    private Material _originalMaterial;

    private void Start()
    {
        // Setup inicial
        if (_targetRenderer != null)
        {
            _originalMaterial = _targetRenderer.material;
        }
        
        // Setup áudio
        if (_audioSource == null)
            _audioSource = GetComponent<AudioSource>();
            
        // Estado inicial
        UpdateVisuals();
        
        // Esconde efeito de hit inicialmente
        if (_hitEffect != null)
            _hitEffect.SetActive(false);
    }

    /// <summary>
    /// Método chamado pelo LightBeamEmitter quando o feixe atinge este alvo
    /// </summary>
    public void OnBeamHit()
    {
        if (_oneTimeActivation && _hasBeenActivatedOnce)
            return;

        if (!_isBeingHit)
        {
            _isBeingHit = true;
            Debug.Log($"Light beam hit target: {gameObject.name}");
            
            // Ativa efeito visual
            if (_hitEffect != null)
                _hitEffect.SetActive(true);
                
            // Toca som de hit
            PlaySound(_hitSound);
            
            // Dispara evento
            onBeamHit?.Invoke();
            
            // Se não requer hit contínuo, ativa imediatamente
            if (!_requiresContinuousHit)
            {
                ActivateTarget();
            }
            else
            {
                // Se requer hit contínuo, inicia coroutine de ativação com delay
                if (_activationCoroutine != null)
                    StopCoroutine(_activationCoroutine);
                    
                _activationCoroutine = StartCoroutine(ContinuousHitActivation());
            }
        }
    }

    /// <summary>
    /// Método chamado quando o feixe para de atingir o alvo
    /// </summary>
    public void OnBeamMiss()
    {
        if (_isBeingHit)
        {
            _isBeingHit = false;
            Debug.Log($"Light beam missed target: {gameObject.name}");
            
            // Desativa efeito visual
            if (_hitEffect != null)
                _hitEffect.SetActive(false);
                
            // Dispara evento
            onBeamMiss?.Invoke();
            
            // Se requer hit contínuo, desativa o alvo
            if (_requiresContinuousHit && _isActivated)
            {
                DeactivateTarget();
            }
            
            // Para coroutine de ativação se estiver rodando
            if (_activationCoroutine != null)
            {
                StopCoroutine(_activationCoroutine);
                _activationCoroutine = null;
            }
        }
    }

    /// <summary>
    /// Ativa o alvo
    /// </summary>
    private void ActivateTarget()
    {
        if (!_isActivated)
        {
            _isActivated = true;
            _hasBeenActivatedOnce = true;
            
            Debug.Log($"Target activated: {gameObject.name}");
            
            // Desabilita o objeto se configurado
            if (_disableOnHit && _objectToDisable != null)
            {
                _objectToDisable.SetActive(false);
                Debug.Log($"Object disabled: {_objectToDisable.name}");
            }
            
            // Toca som de ativação
            PlaySound(_activationSound);
            
            // Atualiza visuais
            UpdateVisuals();
            
            // Dispara evento
            onTargetActivated?.Invoke();
        }
    }

    /// <summary>
    /// Desativa o alvo
    /// </summary>
    private void DeactivateTarget()
    {
        if (_isActivated)
        {
            _isActivated = false;
            
            Debug.Log($"Target deactivated: {gameObject.name}");
            
            // Atualiza visuais
            UpdateVisuals();
            
            // Dispara evento
            onTargetDeactivated?.Invoke();
        }
    }

    /// <summary>
    /// Coroutine para ativação com hit contínuo
    /// </summary>
    private System.Collections.IEnumerator ContinuousHitActivation()
    {
        // Espera o delay de ativação
        if (_activationDelay > 0)
        {
            yield return new WaitForSeconds(_activationDelay);
        }
        
        // Verifica se ainda está sendo atingido
        if (_isBeingHit)
        {
            ActivateTarget();
        }
    }

    /// <summary>
    /// Atualiza os elementos visuais baseado no estado
    /// </summary>
    private void UpdateVisuals()
    {
        // Atualiza luz indicadora
        if (_indicatorLight != null)
        {
            _indicatorLight.enabled = _isActivated;
            _indicatorLight.color = _isActivated ? _activeColor : _inactiveColor;
        }
        
        // Atualiza cor do renderer
        if (_targetRenderer != null)
        {
            Color targetColor = _isActivated ? _activeColor : _inactiveColor;
            _targetRenderer.material.color = targetColor;
        }
    }

    /// <summary>
    /// Toca um som se o AudioSource estiver disponível
    /// </summary>
    private void PlaySound(AudioClip clip)
    {
        if (_audioSource != null && clip != null)
        {
            _audioSource.PlayOneShot(clip);
        }
    }

    /// <summary>
    /// Retorna se o alvo está sendo atingido atualmente
    /// </summary>
    public bool IsBeingHit()
    {
        return _isBeingHit;
    }

    /// <summary>
    /// Retorna se o alvo está ativado
    /// </summary>
    public bool IsActivated()
    {
        return _isActivated;
    }

    /// <summary>
    /// Força a ativação do alvo (método público para uso externo)
    /// </summary>
    public void ForceActivate()
    {
        ActivateTarget();
    }

    /// <summary>
    /// Força a desativação do alvo (método público para uso externo)
    /// </summary>
    public void ForceDeactivate()
    {
        DeactivateTarget();
    }

    /// <summary>
    /// Reseta o alvo para o estado inicial
    /// </summary>
    public void ResetTarget()
    {
        _isActivated = false;
        _isBeingHit = false;
        _hasBeenActivatedOnce = false;
        
        if (_activationCoroutine != null)
        {
            StopCoroutine(_activationCoroutine);
            _activationCoroutine = null;
        }
        
        if (_hitEffect != null)
            _hitEffect.SetActive(false);
        
        // Reativa o objeto se foi desabilitado
        if (_objectToDisable != null)
        {
            _objectToDisable.SetActive(true);
            Debug.Log($"Object re-enabled: {_objectToDisable.name}");
        }
            
        UpdateVisuals();
        
        Debug.Log($"Target reset: {gameObject.name}");
    }

    // Método para debug no inspector
    [ContextMenu("Test Activation")]
    private void TestActivation()
    {
        OnBeamHit();
    }

    [ContextMenu("Test Deactivation")]
    private void TestDeactivation()
    {
        OnBeamMiss();
    }

    // Gizmos para visualização no Scene View
    private void OnDrawGizmos()
    {
        // Desenha uma esfera representando o alvo
        Gizmos.color = _isActivated ? _activeColor : _inactiveColor;
        if (_isBeingHit)
            Gizmos.color = Color.yellow;
            
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        
        // Desenha um ícone indicando que é um alvo
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 0.2f);
    }

    private void OnDrawGizmosSelected()
    {
        // Informações mais detalhadas quando selecionado
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 1f);
        
        // Desenha uma seta apontando para cima indicando que é um alvo
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, Vector3.up * 2f);
    }
} 