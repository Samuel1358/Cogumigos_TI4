using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

/// <summary>
/// Etapa do puzzle com seus elementos específicos
/// </summary>
[System.Serializable]
public class PuzzleStage
{
    [Header("Stage Info")]
    public string stageName = "Stage";
    
    [Header("Stage Elements")]
    public LightBeamEmitter emitter;
    public MirrorReflector[] mirrors;
    public LightBeamTarget target;
    public ActivateSwitch leverSwitch; // ActivateSwitch que fica acessível após completar
    
    [Header("Stage Objects")]
    public Transform[] stageObjects; // Todos os objetos desta etapa (para animação)
    
    [Header("Animation Settings")]
    public float animationDuration = 2f;
    public AnimationCurve movementCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
}

/// <summary>
/// Gerenciador para puzzles de feixe de luz e espelhos com sistema de etapas
/// </summary>
public class LightPuzzleManager : MonoBehaviour
{
    [Header("Puzzle Stages")]
    [SerializeField] private PuzzleStage[] _puzzleStages;
    [SerializeField] private int _currentStageIndex = 0;
    
    [Header("Stage Transition")]
    [SerializeField] private float _sinkDistance = 5f; // Distância para afundar objetos
    [SerializeField] private float _riseDistance = 5f; // Distância para subir objetos
    [SerializeField] private bool _hideInactiveStages = true; // Se true, esconde etapas inativas no início
    
    [Header("Puzzle Settings")]
    [SerializeField] private bool _autoFindElements = false; // Desabilitado para sistema de etapas
    
    [Header("Events")]
    public UnityEvent onPuzzleCompleted;
    public UnityEvent onPuzzleReset;
    public UnityEvent onStageCompleted; // Disparado quando uma etapa é completada
    public UnityEvent onStageStarted; // Disparado quando uma nova etapa inicia
    
    [Header("Debug")]
    [SerializeField] private bool _showDebugInfo = true;
    
    private bool _puzzleCompleted = false;
    private bool _isTransitioning = false;
    private Dictionary<int, Vector3[]> _originalPositions = new Dictionary<int, Vector3[]>(); // Posições originais dos objetos

    private void Start()
    {
        InitializePuzzle();
    }

    /// <summary>
    /// Inicializa o puzzle com sistema de etapas
    /// </summary>
    private void InitializePuzzle()
    {
        if (_puzzleStages == null || _puzzleStages.Length == 0)
        {
            Debug.LogError("Nenhuma etapa configurada no LightPuzzleManager!");
            return;
        }
        
        // Salva posições originais de todos os objetos
        for (int i = 0; i < _puzzleStages.Length; i++)
        {
            var stage = _puzzleStages[i];
            if (stage.stageObjects != null && stage.stageObjects.Length > 0)
            {
                Vector3[] positions = new Vector3[stage.stageObjects.Length];
                for (int j = 0; j < stage.stageObjects.Length; j++)
                {
                    if (stage.stageObjects[j] != null)
                        positions[j] = stage.stageObjects[j].position;
                }
                _originalPositions[i] = positions;
            }
        }
        
        // Configura listeners para todas as etapas
        SetupStageListeners();
        
        // Prepara estado inicial
        if (_hideInactiveStages)
        {
            HideInactiveStages();
        }
        
        // Inicia primeira etapa
        StartStage(_currentStageIndex);
    }

    /// <summary>
    /// Configura os listeners para todas as etapas
    /// </summary>
    private void SetupStageListeners()
    {
        for (int i = 0; i < _puzzleStages.Length; i++)
        {
            var stage = _puzzleStages[i];
            int stageIndex = i; // Captura o índice para a closure
            
            if (stage.target != null)
            {
                stage.target.onTargetActivated.AddListener(() => OnStageTargetActivated(stageIndex));
                stage.target.onTargetDeactivated.AddListener(() => OnStageTargetDeactivated(stageIndex));
            }
            
            // Conecta o ActivateSwitch da alavanca ao sistema de etapas
            if (stage.leverSwitch != null)
            {
                stage.leverSwitch.onActivate.AddListener(() => OnStageLeverActivated(stageIndex));
            }
        }
    }

    /// <summary>
    /// Chamado quando o alvo da etapa atual é ativado
    /// </summary>
    private void OnStageTargetActivated(int stageIndex)
    {
        if (stageIndex != _currentStageIndex || _isTransitioning)
            return;
            
        if (_showDebugInfo)
        {
            Debug.Log($"Stage {stageIndex} target activated: {_puzzleStages[stageIndex].stageName}");
        }
        
        // Não avança automaticamente - espera pela interação com a alavanca
    }

    /// <summary>
    /// Chamado quando o alvo da etapa atual é desativado
    /// </summary>
    private void OnStageTargetDeactivated(int stageIndex)
    {
        if (stageIndex != _currentStageIndex || _isTransitioning)
            return;
            
        if (_showDebugInfo)
        {
            Debug.Log($"Stage {stageIndex} target deactivated: {_puzzleStages[stageIndex].stageName}");
        }
    }

    /// <summary>
    /// Inicia uma etapa específica do puzzle - ativa objetos e mostra animação
    /// </summary>
    private void StartStage(int stageIndex)
    {
        if (stageIndex < 0 || stageIndex >= _puzzleStages.Length)
        {
            Debug.LogError($"Índice de etapa inválido: {stageIndex}");
            return;
        }
        
        _currentStageIndex = stageIndex;
        var currentStage = _puzzleStages[_currentStageIndex];
        
        // Ativa todos os objetos da etapa atual
        if (currentStage.stageObjects != null)
        {
            foreach (var obj in currentStage.stageObjects)
            {
                if (obj != null) 
                {
                    obj.gameObject.SetActive(true);
                }
            }
        }
        
        // Inicia animação de subida se não for a primeira etapa
        if (stageIndex > 0)
        {
            StartCoroutine(RiseStageCoroutine(stageIndex));
        }
        
        if (_showDebugInfo)
        {
            Debug.Log($"Starting stage {stageIndex}: {currentStage.stageName}");
        }
        
        // Ativa apenas o emissor da etapa atual
        if (currentStage.emitter != null)
        {
            // Desativa todos os outros emissores
            for (int i = 0; i < _puzzleStages.Length; i++)
            {
                if (i != stageIndex && _puzzleStages[i].emitter != null)
                {
                    _puzzleStages[i].emitter.DeactivateBeam();
                }
            }
        }
        
        onStageStarted?.Invoke();
    }

    /// <summary>
    /// Chamado quando a alavanca da etapa é ativada
    /// </summary>
    public void OnStageLeverActivated(int stageIndex)
    {
        // Só processa se for a etapa atual e não estiver em transição
        if (stageIndex != _currentStageIndex || _isTransitioning)
        {
            if (_showDebugInfo)
            {
                Debug.Log($"Lever activated for stage {stageIndex}, but current stage is {_currentStageIndex} or transitioning");
            }
            return;
        }
            
        var currentStage = _puzzleStages[_currentStageIndex];
        
        if (_showDebugInfo)
        {
            Debug.Log($"Stage {_currentStageIndex} lever activated: {currentStage.stageName}");
        }
        
        CompleteCurrentStage();
    }

    /// <summary>
    /// Método público para compatibilidade (pode ser chamado via UnityEvent se necessário)
    /// </summary>
    public void OnStageLeverActivated()
    {
        OnStageLeverActivated(_currentStageIndex);
    }

    /// <summary>
    /// Completa a etapa atual e avança para a próxima
    /// </summary>
    private void CompleteCurrentStage()
    {
        if (_isTransitioning)
            return;
            
        onStageCompleted?.Invoke();
        
        if (_showDebugInfo)
        {
            Debug.Log($"Stage {_currentStageIndex} completed!");
        }
        
        // Verifica se é a última etapa
        if (_currentStageIndex >= _puzzleStages.Length - 1)
        {
            CompletePuzzle();
        }
        else
        {
            TransitionToNextStage();
        }
    }

    /// <summary>
    /// Faz a transição para a próxima etapa
    /// </summary>
    private void TransitionToNextStage()
    {
        StartCoroutine(TransitionStageCoroutine(_currentStageIndex, _currentStageIndex + 1));
    }

    /// <summary>
    /// Completa todo o puzzle
    /// </summary>
    private void CompletePuzzle()
    {
        _puzzleCompleted = true;
        onPuzzleCompleted?.Invoke();
        
        if (_showDebugInfo)
        {
            Debug.Log("PUZZLE COMPLETED!");
        }
    }

    /// <summary>
    /// Corrotina para animar objetos de uma etapa subindo
    /// </summary>
    private System.Collections.IEnumerator RiseStageCoroutine(int stageIndex)
    {
        if (stageIndex < 0 || stageIndex >= _puzzleStages.Length)
            yield break;
            
        var stage = _puzzleStages[stageIndex];
        if (stage.stageObjects == null || stage.stageObjects.Length == 0)
            yield break;
        
        float duration = stage.animationDuration;
        float elapsedTime = 0f;
        
        // Posições inicial (abaixo) e final (original)
        Vector3[] startPositions = new Vector3[stage.stageObjects.Length];
        Vector3[] endPositions = new Vector3[stage.stageObjects.Length];
        
        for (int i = 0; i < stage.stageObjects.Length; i++)
        {
            if (stage.stageObjects[i] != null)
            {
                startPositions[i] = _originalPositions[stageIndex][i] + Vector3.down * _riseDistance;
                endPositions[i] = _originalPositions[stageIndex][i];
                
                // Posiciona abaixo antes de começar
                stage.stageObjects[i].position = startPositions[i];
            }
        }
        
        // Animação de subida
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / duration;
            
            float curveValue = normalizedTime;
            if (stage.movementCurve != null && stage.movementCurve.keys.Length > 0)
            {
                curveValue = stage.movementCurve.Evaluate(normalizedTime);
            }
            
            for (int i = 0; i < stage.stageObjects.Length; i++)
            {
                if (stage.stageObjects[i] != null)
                {
                    stage.stageObjects[i].position = Vector3.Lerp(startPositions[i], endPositions[i], curveValue);
                }
            }
            
            yield return null;
        }
        
        // Garante posição final
        for (int i = 0; i < stage.stageObjects.Length; i++)
        {
            if (stage.stageObjects[i] != null)
            {
                stage.stageObjects[i].position = endPositions[i];
            }
        }
        
        if (_showDebugInfo)
        {
            Debug.Log($"Stage {stageIndex} rise animation completed: {stage.stageName}");
        }
    }

    /// <summary>
    /// Coroutine para fazer a transição entre etapas
    /// </summary>
    private System.Collections.IEnumerator TransitionStageCoroutine(int fromStage, int toStage)
    {
        _isTransitioning = true;
        
        if (_showDebugInfo)
        {
            Debug.Log($"Transitioning from stage {fromStage} to stage {toStage}");
        }
        
        var currentStageObjects = _puzzleStages[fromStage].stageObjects;
        var animDuration = _puzzleStages[fromStage].animationDuration;
        var curve = _puzzleStages[fromStage].movementCurve;
        
        // Desativa emissor da etapa atual
        if (_puzzleStages[fromStage].emitter != null)
        {
            _puzzleStages[fromStage].emitter.DeactivateBeam();
        }
        
        float elapsedTime = 0f;
        
        // Posições iniciais e finais para etapa atual (afundar)
        Vector3[] currentStageStartPos = new Vector3[currentStageObjects.Length];
        Vector3[] currentStageEndPos = new Vector3[currentStageObjects.Length];
        
        // Calcula posições para etapa atual (afundar)
        for (int i = 0; i < currentStageObjects.Length; i++)
        {
            if (currentStageObjects[i] != null)
            {
                currentStageStartPos[i] = currentStageObjects[i].position;
                currentStageEndPos[i] = currentStageStartPos[i] + Vector3.down * _sinkDistance;
            }
        }
        
        // Animação de afundamento da etapa atual
        while (elapsedTime < animDuration)
        {
            float progress = elapsedTime / animDuration;
            float curveProgress = curve.Evaluate(progress);
            
            // Move objetos da etapa atual para baixo
            for (int i = 0; i < currentStageObjects.Length; i++)
            {
                if (currentStageObjects[i] != null)
                {
                    currentStageObjects[i].position = Vector3.Lerp(currentStageStartPos[i], currentStageEndPos[i], curveProgress);
                }
            }
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // Garante posições finais e desabilita objetos da etapa completada
        for (int i = 0; i < currentStageObjects.Length; i++)
        {
            if (currentStageObjects[i] != null)
            {
                currentStageObjects[i].position = currentStageEndPos[i];
                currentStageObjects[i].gameObject.SetActive(false);
            }
        }
        
        if (_showDebugInfo)
        {
            Debug.Log($"Stage {fromStage} objects disabled after animation");
        }
        
        _isTransitioning = false;
        
        // Inicia a próxima etapa
        StartStage(toStage);
    }

    /// <summary>
    /// Esconde etapas inativas no início (desabilita objetos)
    /// </summary>
    private void HideInactiveStages()
    {
        for (int i = 0; i < _puzzleStages.Length; i++)
        {
            if (i != _currentStageIndex)
            {
                var stage = _puzzleStages[i];
                if (stage.stageObjects != null)
                {
                    foreach (var obj in stage.stageObjects)
                    {
                        if (obj != null)
                        {
                            // Posiciona abaixo e desabilita
                            obj.position = _originalPositions[i][System.Array.IndexOf(stage.stageObjects, obj)] + Vector3.down * _riseDistance;
                            obj.gameObject.SetActive(false);
                        }
                    }
                }
                
                if (_showDebugInfo)
                {
                    Debug.Log($"Stage {i} objects disabled and hidden: {stage.stageName}");
                }
            }
        }
    }

    /// <summary>
    /// Reseta o puzzle para o estado inicial
    /// </summary>
    public void ResetPuzzle()
    {
        StopAllCoroutines();
        _isTransitioning = false;
        _currentStageIndex = 0;
        _puzzleCompleted = false;
        
        // Restaura posições originais e ativa todos os objetos primeiro
        for (int i = 0; i < _puzzleStages.Length; i++)
        {
            var stage = _puzzleStages[i];
            if (stage.stageObjects != null && _originalPositions.ContainsKey(i))
            {
                for (int j = 0; j < stage.stageObjects.Length; j++)
                {
                    if (stage.stageObjects[j] != null && j < _originalPositions[i].Length)
                    {
                        stage.stageObjects[j].gameObject.SetActive(true);
                        stage.stageObjects[j].position = _originalPositions[i][j];
                    }
                }
            }
            
            // Reseta alvos
            if (stage.target != null)
            {
                stage.target.ResetTarget();
            }
            
            // Desativa emissores
            if (stage.emitter != null)
            {
                stage.emitter.DeactivateBeam();
            }
        }
        
        onPuzzleReset?.Invoke();
        
        // Reinicia - primeiro esconde etapas inativas, depois inicia a primeira
        if (_hideInactiveStages)
        {
            HideInactiveStages();
        }
        StartStage(0);
        
        if (_showDebugInfo)
        {
            Debug.Log("Puzzle reset to initial state");
        }
    }

    /// <summary>
    /// Força a completar a etapa atual (para teste)
    /// </summary>
    [ContextMenu("Force Complete Current Stage")]
    public void ForceCompleteCurrentStage()
    {
        if (_currentStageIndex < _puzzleStages.Length)
        {
            var currentStage = _puzzleStages[_currentStageIndex];
            if (currentStage.target != null)
            {
                currentStage.target.ForceActivate();
            }
            CompleteCurrentStage();
        }
    }

    /// <summary>
    /// Força a completar todo o puzzle (para teste)
    /// </summary>
    [ContextMenu("Force Complete Puzzle")]
    public void ForceCompletePuzzle()
    {
        _currentStageIndex = _puzzleStages.Length - 1;
        CompletePuzzle();
    }

    /// <summary>
    /// Avança manualmente para a próxima etapa (para teste)
    /// </summary>
    [ContextMenu("Go to Next Stage")]
    public void GoToNextStage()
    {
        if (_currentStageIndex < _puzzleStages.Length - 1)
        {
            TransitionToNextStage();
        }
    }

    /// <summary>
    /// Retorna o progresso atual do puzzle (0.0 a 1.0)
    /// </summary>
    public float GetProgress()
    {
        if (_puzzleStages.Length == 0) return 0f;
        return (float)_currentStageIndex / (_puzzleStages.Length - 1);
    }

    /// <summary>
    /// Retorna se o puzzle está completado
    /// </summary>
    public bool IsPuzzleCompleted()
    {
        return _puzzleCompleted;
    }

    /// <summary>
    /// Retorna a etapa atual
    /// </summary>
    public int GetCurrentStageIndex()
    {
        return _currentStageIndex;
    }

    /// <summary>
    /// Retorna o total de etapas
    /// </summary>
    public int GetTotalStages()
    {
        return _puzzleStages.Length;
    }

    /// <summary>
    /// Retorna se está em transição entre etapas
    /// </summary>
    public bool IsTransitioning()
    {
        return _isTransitioning;
    }

    /// <summary>
    /// Retorna a etapa atual
    /// </summary>
    public PuzzleStage GetCurrentStage()
    {
        if (_currentStageIndex >= 0 && _currentStageIndex < _puzzleStages.Length)
        {
            return _puzzleStages[_currentStageIndex];
        }
        return null;
    }

    // Métodos para uso no inspector/debug
    [ContextMenu("Reset Puzzle")]
    private void ResetPuzzleMenuItem()
    {
        ResetPuzzle();
    }

    // Informações de debug no inspector
    private void OnValidate()
    {
        if (Application.isPlaying && _showDebugInfo)
        {
            // Validações básicas para evitar erros
            if (_puzzleStages != null)
            {
                for (int i = 0; i < _puzzleStages.Length; i++)
                {
                    if (_puzzleStages[i] == null)
                    {
                        _puzzleStages[i] = new PuzzleStage();
                    }
                }
            }
        }
    }
} 