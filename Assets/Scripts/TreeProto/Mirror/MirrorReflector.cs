using UnityEngine;

/// <summary>
/// Estados de reflexão disponíveis para o espelho (apenas 2 estados)
/// </summary>
public enum MirrorState
{
    ReflectRight = 0,   // Rotação visual +45° Y, reflete para a esquerda (-90°)
    ReflectLeft = 1     // Rotação visual -45° Y, reflete para a direita (+90°)
}

public class MirrorReflector : MonoBehaviour
{
    [Header("Mirror Settings")]
    [SerializeField] private MirrorState _currentState = MirrorState.ReflectRight;
    
    [Header("Visual Feedback")]
    [SerializeField] private GameObject _reflectionEffect; // Efeito visual opcional quando o feixe bate
    [SerializeField] private float _effectDuration = 0.5f;
    
    [Header("Rotation Animation")]
    [SerializeField] private float _rotationAnimationDuration = 0.5f; // Duração da animação de rotação
    [SerializeField] private AnimationCurve _rotationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // Curva de animação
    
    [Header("Debug")]
    [SerializeField] private bool _showDebugRays = true;
    [SerializeField] private float _debugRayLength = 2f;
    
    private bool _isBeingHit = false;
    private bool _isRotating = false;
    private Coroutine _effectCoroutine;
    private Coroutine _rotationCoroutine;

    private void Start()
    {
        // Aplica a rotação inicial baseada no estado
        UpdateVisualRotation();
        
        // Esconde o efeito visual inicialmente
        if (_reflectionEffect != null)
            _reflectionEffect.SetActive(false);
    }

    /// <summary>
    /// Calcula a direção refletida do feixe de luz baseada no estado atual
    /// </summary>
    /// <param name="incomingDirection">Direção do feixe que está chegando</param>
    /// <param name="surfaceNormal">Normal da superfície onde o feixe bateu (do Raycast)</param>
    /// <returns>Direção refletida</returns>
    public Vector3 GetReflectedDirection(Vector3 incomingDirection, Vector3 surfaceNormal)
    {
        Vector3 reflectedDirection;
        
        // Calcula reflexão de 90 graus baseada no estado atual (invertido)
        if (_currentState == MirrorState.ReflectRight)
        {
            // Rotação visual +45° Y, mas reflete para a esquerda (-90 graus)
            reflectedDirection = Quaternion.AngleAxis(-90f, Vector3.up) * incomingDirection;
        }
        else
        {
            // Rotação visual -45° Y, mas reflete para a direita (+90 graus)
            reflectedDirection = Quaternion.AngleAxis(90f, Vector3.up) * incomingDirection;
        }
        
        // Ativa efeito visual
        TriggerReflectionEffect();
        
        return reflectedDirection.normalized;
    }

    /// <summary>
    /// Ativa o efeito visual quando o feixe bate no espelho
    /// </summary>
    private void TriggerReflectionEffect()
    {
        if (_reflectionEffect != null)
        {
            // Para o coroutine anterior se estiver rodando
            if (_effectCoroutine != null)
            {
                StopCoroutine(_effectCoroutine);
            }
            
            _effectCoroutine = StartCoroutine(ShowReflectionEffect());
        }
        
        _isBeingHit = true;
    }

    /// <summary>
    /// Coroutine para mostrar o efeito visual temporariamente
    /// </summary>
    private System.Collections.IEnumerator ShowReflectionEffect()
    {
        _reflectionEffect.SetActive(true);
        yield return new WaitForSeconds(_effectDuration);
        _reflectionEffect.SetActive(false);
        _isBeingHit = false;
    }

    /// <summary>
    /// Alterna entre os dois estados do espelho
    /// </summary>
    public void ToggleMirrorState()
    {
        if (_isRotating) return; // Impede rotação durante animação
        
        MirrorState newState = (_currentState == MirrorState.ReflectRight) ? 
                               MirrorState.ReflectLeft : 
                               MirrorState.ReflectRight;
        
        SetMirrorStateAnimated(newState);
    }

    /// <summary>
    /// Define o estado do espelho sem animação
    /// </summary>
    public void SetMirrorState(MirrorState newState)
        {
        _currentState = newState;
        UpdateVisualRotation();
    }
    
    /// <summary>
    /// Define o estado do espelho com animação
    /// </summary>
    public void SetMirrorStateAnimated(MirrorState newState)
    {
        if (_isRotating || _currentState == newState) return;
        
        if (_rotationCoroutine != null)
        {
            StopCoroutine(_rotationCoroutine);
        }
        
        _rotationCoroutine = StartCoroutine(RotateToStateCoroutine(newState));
    }

    /// <summary>
    /// Atualiza a rotação visual do objeto baseada no estado atual
    /// </summary>
    private void UpdateVisualRotation()
    {
        float targetRotationY = (_currentState == MirrorState.ReflectRight) ? 45f : -45f;
        
        // Aplica a rotação no eixo Y (mantém X e Z inalterados)
        Vector3 currentEuler = transform.localEulerAngles;
        transform.localRotation = Quaternion.Euler(currentEuler.x, targetRotationY, currentEuler.z);
    }

    /// <summary>
    /// Corrotina para animação de rotação entre estados
    /// </summary>
    private System.Collections.IEnumerator RotateToStateCoroutine(MirrorState targetState)
    {
        _isRotating = true;
        
        // Pega a rotação atual
        Vector3 startRotation = transform.localEulerAngles;
        float targetRotationY = (targetState == MirrorState.ReflectRight) ? 45f : -45f;
        Vector3 targetRotation = new Vector3(startRotation.x, targetRotationY, startRotation.z);
        
        float elapsedTime = 0f;
        
        while (elapsedTime < _rotationAnimationDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / _rotationAnimationDuration;
            
            // Aplica a curva de animação
            float curveValue = _rotationCurve.Evaluate(normalizedTime);
            
            // Interpola entre rotação inicial e final
            Vector3 currentRotation = Vector3.Lerp(startRotation, targetRotation, curveValue);
            
            // Aplica a rotação
            transform.localRotation = Quaternion.Euler(currentRotation);
            
            yield return null;
        }
        
        // Garante a rotação final exata
        transform.localRotation = Quaternion.Euler(targetRotation);
        
        // Atualiza o estado
        _currentState = targetState;
        
        _isRotating = false;
    }
    
    /// <summary>
    /// Retorna se o espelho está sendo atingido por um feixe atualmente
    /// </summary>
    public bool IsBeingHit()
        {
        return _isBeingHit;
    }

    /// <summary>
    /// Retorna se o espelho está atualmente rotacionando
    /// </summary>
    public bool IsRotating()
            {
        return _isRotating;
    }

    /// <summary>
    /// Retorna o estado atual do espelho
    /// </summary>
    public MirrorState GetCurrentState()
    {
        return _currentState;
    }

    /// <summary>
    /// Retorna a rotação Y atual baseada no estado
    /// </summary>
    public float GetCurrentRotationY()
    {
        return (_currentState == MirrorState.ReflectRight) ? 45f : -45f;
    }

    // Métodos para teste no inspector
    [ContextMenu("Set State: Reflect Right (45° Y)")]
    private void SetStateRight()
    {
        SetMirrorStateAnimated(MirrorState.ReflectRight);
    }

    [ContextMenu("Set State: Reflect Left (-45° Y)")]
    private void SetStateLeft()
    {
        SetMirrorStateAnimated(MirrorState.ReflectLeft);
    }

    [ContextMenu("Toggle State (Animated)")]
    private void ToggleStateMenu()
    {
        ToggleMirrorState();
    }

    [ContextMenu("Update Visual Rotation")]
    private void UpdateVisualRotationMenu()
    {
        UpdateVisualRotation();
    }

    // Métodos de debug para visualizar no Scene View
    private void OnDrawGizmos()
    {
        if (_showDebugRays)
        {
            // Desenha uma representação do espelho
            Gizmos.color = _isBeingHit ? Color.yellow : Color.white;
            
            // Desenha uma linha representando a superfície do espelho
            Vector3 right = transform.right;
            float size = 0.5f;
            
            Gizmos.DrawLine(transform.position - right * size, transform.position + right * size);
            
            // Desenha uma seta indicando a direção de reflexão
            Vector3 reflectionDir = (_currentState == MirrorState.ReflectRight) ? 
                                  transform.up : -transform.up;
            
            Gizmos.color = (_currentState == MirrorState.ReflectRight) ? Color.green : Color.red;
            Gizmos.DrawRay(transform.position, reflectionDir * _debugRayLength * 0.5f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Desenha informações mais detalhadas quando selecionado
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.1f);
        
        // Desenha seta azul mostrando a direção de entrada padrão
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position - transform.forward * _debugRayLength * 0.7f, 
                      transform.forward * _debugRayLength * 0.7f);
        
        // Desenha seta mostrando a direção de reflexão baseada no estado (invertido)
        Vector3 incomingDirection = Vector3.forward; // Direção padrão de entrada
        Vector3 reflectedDirection;
        
        if (_currentState == MirrorState.ReflectRight)
        {
            reflectedDirection = Quaternion.AngleAxis(-90f, Vector3.up) * incomingDirection;
            Gizmos.color = Color.red; // Vermelho para esquerda
        }
        else
        {
            reflectedDirection = Quaternion.AngleAxis(90f, Vector3.up) * incomingDirection;
            Gizmos.color = Color.green; // Verde para direita
        }
        
        Gizmos.DrawRay(transform.position, reflectedDirection * _debugRayLength * 0.7f);
        
        // Desenha texto com informações
        #if UNITY_EDITOR
        UnityEditor.Handles.Label(transform.position + Vector3.up * 1f, 
            $"State: {_currentState}\nY Rotation: {GetCurrentRotationY()}°\nReflection: {(_currentState == MirrorState.ReflectRight ? "Left (-90°)" : "Right (+90°)")}");
        #endif
    }
} 