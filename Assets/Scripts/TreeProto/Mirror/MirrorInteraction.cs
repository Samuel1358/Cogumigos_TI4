using UnityEngine;

/// <summary>
/// Script de interação para espelhos - integra com InteractingArea
/// Este script deve ser anexado ao objeto pai (base) do espelho
/// Segue o padrão do sistema existente (como ActivateSwitch)
/// </summary>
public class MirrorInteraction : MonoBehaviour
{
    [Header("Mirror Reference")]
    [SerializeField] private MirrorReflector _mirrorReflector; // Referência para o script do espelho
    
    [Header("Interaction Settings")]
    [SerializeField] private InteractingArea _area; // Referência para o InteractingArea
    [SerializeField] private bool _autoFindMirror = true; // Se true, procura automaticamente o MirrorReflector nos filhos
    [SerializeField] private bool _interactJustOnce = false; // Se true, só permite uma interação
    
    [Header("Audio Feedback")]
    [SerializeField] private float _volume = 0.7f; // Volume do som
    
    private bool _useFirstSound = true; // Alterna entre os dois sons
    
    [Header("Debug")]
    [SerializeField] private bool _showDebugInfo = true;

    private MirrorInteractionScript _interaction;

    private void Awake()
    {
        InitializeInteraction();
    }

    /// <summary>
    /// Inicializa o sistema de interação seguindo o padrão do ActivateSwitch
    /// </summary>
    private void InitializeInteraction()
    {
        // Cria a instância da interação
        _interaction = ScriptableObject.CreateInstance<MirrorInteractionScript>();
        _interaction.Assign(GetMirrorReflector(), HandleMirrorInteraction);

        // Configura se a interação deve acontecer apenas uma vez
        _interaction.SetInteractJustOnce(_interactJustOnce);

        // Procura InteractingArea se não configurado
        if (_area == null)
        {
            _area = GetComponent<InteractingArea>();
        }

        if (_area != null)
        {
            // Associa a interação ao InteractingArea
            _area.Assign(_interaction);
        }
        else
        {
            Debug.LogError($"MirrorInteraction: Nenhum InteractingArea encontrado em {gameObject.name}! " +
                          "Adicione um InteractingArea component ao objeto base do espelho.");
        }

        // Audio será gerenciado pelo AudioManager.Instance
    }

    /// <summary>
    /// Obtém o MirrorReflector (com busca automática se necessário)
    /// </summary>
    private MirrorReflector GetMirrorReflector()
    {
        // Procura MirrorReflector automaticamente se necessário
        if (_autoFindMirror && _mirrorReflector == null)
        {
            _mirrorReflector = GetComponentInChildren<MirrorReflector>();
            if (_mirrorReflector == null)
            {
                Debug.LogError($"MirrorInteraction: Nenhum MirrorReflector encontrado nos filhos de {gameObject.name}! " +
                              "Certifique-se de que o espelho está estruturado corretamente.");
            }
        }

        return _mirrorReflector;
    }

    /// <summary>
    /// Chamado quando o jogador interage com o espelho (via sistema Interaction)
    /// </summary>
    private void HandleMirrorInteraction(MirrorReflector mirrorReflector)
    {
        if (mirrorReflector == null)
        {
            Debug.LogWarning($"MirrorInteraction: Tentativa de interação mas nenhum MirrorReflector configurado em {gameObject.name}");
            return;
        }

        // Impede interação durante rotação
        if (mirrorReflector.IsRotating())
        {
            return;
        }

        // Toca som de rotação
        PlayRotationSound();

        // Rotaciona o espelho (usando novo sistema)
        mirrorReflector.ToggleMirrorState();

        if (_showDebugInfo)
        {
            Debug.Log($"Player interacted with mirror {gameObject.name} - New state: {mirrorReflector.GetCurrentState()}");
        }
    }

    /// <summary>
    /// Toca o som de rotação alternando entre os dois sons
    /// </summary>
    private void PlayRotationSound()
    {
        if (GameIniciator.Instance.AudioManagerInstance != null)
        {
            // Alterna entre os dois sons
            string soundName = _useFirstSound ? SoundEffectNames.ESPELHO_MEXENDO : SoundEffectNames.ESPELHO_MEXENDO2;
            
            GameIniciator.Instance.AudioManagerInstance.PlaySFX(soundName);
            
            // Alterna para o próximo som
            _useFirstSound = !_useFirstSound;
        }
        else
        {
            Debug.LogWarning("GameIniciator.Instance.AudioManagerInstance is null - cannot play mirror rotation sound");
        }
    }

    /// <summary>
    /// Define o MirrorReflector manualmente
    /// </summary>
    public void SetMirrorReflector(MirrorReflector mirrorReflector)
    {
        _mirrorReflector = mirrorReflector;
    }

    /// <summary>
    /// Retorna o MirrorReflector atual
    /// </summary>
    public MirrorReflector GetMirrorReflectorComponent()
    {
        return GetMirrorReflector();
    }

    /// <summary>
    /// Força uma rotação do espelho (útil para scripts externos)
    /// </summary>
    public void ForceRotateMirror()
    {
        HandleMirrorInteraction(GetMirrorReflector());
    }

    /// <summary>
    /// Define um estado específico do espelho
    /// </summary>
    public void SetMirrorState(MirrorState state)
    {
        var mirror = GetMirrorReflector();
        if (mirror != null)
        {
            PlayRotationSound();
            mirror.SetMirrorStateAnimated(state);
        }
    }

    /// <summary>
    /// Retorna se o espelho está atualmente rotacionando
    /// </summary>
    public bool IsMirrorRotating()
    {
        var mirror = GetMirrorReflector();
        return mirror != null && mirror.IsRotating();
    }

    /// <summary>
    /// Retorna o estado atual do espelho
    /// </summary>
    public MirrorState GetCurrentMirrorState()
    {
        var mirror = GetMirrorReflector();
        return mirror != null ? mirror.GetCurrentState() : MirrorState.ReflectRight;
    }

    // Métodos para uso no inspector/debug
    [ContextMenu("Test Interaction")]
    private void TestInteraction()
    {
        HandleMirrorInteraction(GetMirrorReflector());
    }

    [ContextMenu("Set State: Reflect Right")]
    private void SetStateRight()
    {
        SetMirrorState(MirrorState.ReflectRight);
    }

    [ContextMenu("Set State: Reflect Left")]
    private void SetStateLeft()
    {
        SetMirrorState(MirrorState.ReflectLeft);
    }

    [ContextMenu("Toggle Mirror State")]
    private void ToggleState()
    {
        var mirror = GetMirrorReflector();
        if (mirror != null)
        {
            PlayRotationSound();
            mirror.ToggleMirrorState();
        }
    }

    private void OnValidate()
    {
        // Validações no editor
        if (_autoFindMirror && _mirrorReflector == null && Application.isPlaying)
        {
            _mirrorReflector = GetComponentInChildren<MirrorReflector>();
        }
    }
} 