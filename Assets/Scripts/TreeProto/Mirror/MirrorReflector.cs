using UnityEngine;

/// <summary>
/// Direções de reflexão disponíveis para o espelho (4 direções possíveis)
/// </summary>
public enum ReflectionDirection
{
    Right = 90,      // 90 graus para a direita (sentido horário)
    Down = 180,      // 180 graus para baixo
    Left = -90,      // 90 graus para a esquerda (sentido anti-horário)  
    Up = 0           // 0 graus (direção original/para cima)
}

public class MirrorReflector : MonoBehaviour
{
    [Header("Mirror Settings")]
    [SerializeField] private Vector3 _mirrorNormal = Vector3.up; // Normal da superfície do espelho (editável no inspector)
    [SerializeField] private bool _use90DegreeReflection = true; // Se true, sempre reflete em 90 graus
    [SerializeField] private bool _useTransformForward = false; // Se true, usa transform.forward como normal
    
    [Header("Reflection Direction")]
    [SerializeField] private ReflectionDirection _reflectionDirection = ReflectionDirection.Right; // Direção da reflexão
    [SerializeField] private float _customReflectionAngle = 90f; // Ângulo customizado se não usar direções pré-definidas
    [SerializeField] private bool _useCustomAngle = false; // Se true, usa o ângulo customizado ao invés das direções pré-definidas
    
    [Header("Visual Feedback")]
    [SerializeField] private GameObject _reflectionEffect; // Efeito visual opcional quando o feixe bate
    [SerializeField] private float _effectDuration = 0.5f;
    
    [Header("Rotation Animation")]
    [SerializeField] private float _rotationAnimationDuration = 1f; // Duração da animação de rotação
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
        // Se usar transform.forward, atualiza o normal baseado na rotação
        if (_useTransformForward)
        {
            _mirrorNormal = transform.forward;
        }
        
        // Normaliza o vetor normal
        _mirrorNormal = _mirrorNormal.normalized;
        
        // Sincroniza direção com rotação atual do pai
        UpdateReflectionDirectionFromParentRotation();
        
        // Esconde o efeito visual inicialmente
        if (_reflectionEffect != null)
            _reflectionEffect.SetActive(false);
    }

    private void Update()
    {
        // Atualiza o normal se estiver usando transform.forward
        if (_useTransformForward)
        {
            _mirrorNormal = transform.forward.normalized;
        }
    }

    /// <summary>
    /// Calcula a direção refletida do feixe de luz
    /// </summary>
    /// <param name="incomingDirection">Direção do feixe que está chegando</param>
    /// <param name="surfaceNormal">Normal da superfície onde o feixe bateu (do Raycast)</param>
    /// <returns>Direção refletida</returns>
    public Vector3 GetReflectedDirection(Vector3 incomingDirection, Vector3 surfaceNormal)
    {
        Vector3 reflectedDirection;
        
        if (_use90DegreeReflection)
        {
            // Reflexão controlada em 90 graus baseada na rotação do pai
            reflectedDirection = CalculateControlled90DegreeReflection(incomingDirection);
            
            // Debug para verificar
            Debug.Log($"Mirror {gameObject.name}: incoming {incomingDirection} -> reflected {reflectedDirection} (parent rotation: {GetParentRotationY()}°)");
        }
        else
        {
            // Reflexão física normal (ângulo de incidência = ângulo de reflexão)
            Vector3 mirrorNormal = GetEffectiveMirrorNormal();
            reflectedDirection = Vector3.Reflect(incomingDirection, mirrorNormal);
        }
        
        // Ativa efeito visual
        TriggerReflectionEffect();
        
        return reflectedDirection.normalized;
    }

    /// <summary>
    /// Calcula reflexão controlada de 90 graus baseada na rotação do pai
    /// Espelho sempre reflete em 90°, alternando o lado a cada rotação
    /// </summary>
    private Vector3 CalculateControlled90DegreeReflection(Vector3 incomingDirection)
    {
        // Pega a rotação Y atual do pai
        float parentRotationY = GetParentRotationY();
        
        // Normaliza a rotação para 0-360
        parentRotationY = ((parentRotationY % 360) + 360) % 360;
        
        // Determina se reflete para o lado positivo ou negativo (90° ou -90°)
        // A cada 90° de rotação do pai, alterna entre +90° e -90°
        bool reflectPositive = ShouldReflectPositive(parentRotationY);
        
        // Sempre reflete em 90 graus, mas alterna o lado
        float reflectionAngle = reflectPositive ? 90f : -90f;
        
        // Aplica a rotação de 90 graus (sempre perpendicular)
        Vector3 reflectedDirection = Quaternion.AngleAxis(reflectionAngle, Vector3.up) * incomingDirection;
        
        Debug.Log($"Mirror parent at {parentRotationY:F1}°, reflecting {(reflectPositive ? "+90°" : "-90°")} - incoming: {incomingDirection} -> reflected: {reflectedDirection}");
        
        return reflectedDirection;
    }

    /// <summary>
    /// Determina se deve refletir no sentido positivo (+90°) ou negativo (-90°)
    /// baseado na rotação do pai
    /// </summary>
    private bool ShouldReflectPositive(float parentRotationY)
    {
        // Divide as rotações em grupos de 90°
        // Alterna entre positivo e negativo a cada rotação
        int rotationQuadrant = Mathf.FloorToInt(parentRotationY / 90f);
        
        // Quadrantes pares = +90°, ímpares = -90°
        // 0° (quadrante 0) = +90°
        // 90° (quadrante 1) = -90°  
        // 180° (quadrante 2) = +90°
        // 270° (quadrante 3) = -90°
        return rotationQuadrant % 2 == 0;
    }

    /// <summary>
    /// Obtém a rotação Y efetiva do pai (ou própria se não tiver pai)
    /// </summary>
    private float GetParentRotationY()
    {
        if (transform.parent != null)
        {
            return transform.parent.localEulerAngles.y;
        }
        return transform.localEulerAngles.y;
    }

    /// <summary>
    /// Obtém a normal efetiva do espelho considerando rotação do pai
    /// </summary>
    private Vector3 GetEffectiveMirrorNormal()
    {
        if (_useTransformForward)
        {
            // Se usar transform forward, considera a rotação do pai
            if (transform.parent != null)
            {
                return transform.parent.TransformDirection(Vector3.forward).normalized;
            }
            return transform.forward.normalized;
        }
        else
        {
            // Se usar normal configurável, aplica rotação do pai se existir
            if (transform.parent != null)
            {
                return transform.parent.TransformDirection(_mirrorNormal).normalized;
            }
            return _mirrorNormal.normalized;
        }
    }

    /// <summary>
    /// Atualiza a direção de reflexão baseada na rotação atual do pai
    /// Apenas para display/debug - a reflexão real usa ShouldReflectPositive()
    /// </summary>
    private void UpdateReflectionDirectionFromParentRotation()
    {
        float parentRotationY = GetParentRotationY();
        
        // Normaliza a rotação para 0-360
        parentRotationY = ((parentRotationY % 360) + 360) % 360;
        
        // Atualiza apenas para referência visual/debug
        // A reflexão real sempre é +90° ou -90° alternando
        int rotationQuadrant = Mathf.FloorToInt(parentRotationY / 90f);
        
        switch (rotationQuadrant)
        {
            case 0: // 0°-89°
                _reflectionDirection = ReflectionDirection.Up; // +90°
                break;
            case 1: // 90°-179°
                _reflectionDirection = ReflectionDirection.Right; // -90°
                break;
            case 2: // 180°-269°
                _reflectionDirection = ReflectionDirection.Down; // +90°
                break;
            case 3: // 270°-359°
                _reflectionDirection = ReflectionDirection.Left; // -90°
                break;
        }
        
        bool isPositive = ShouldReflectPositive(parentRotationY);
        Debug.Log($"Mirror at {parentRotationY:F1}° - Direction: {_reflectionDirection}, Reflects: {(isPositive ? "+90°" : "-90°")}");
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
        Debug.Log($"Mirror {gameObject.name} reflecting light beam");
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
    /// Define o normal do espelho programaticamente
    /// </summary>
    public void SetMirrorNormal(Vector3 normal)
    {
        _mirrorNormal = normal.normalized;
        _useTransformForward = false;
    }

    /// <summary>
    /// Rotaciona o espelho para uma nova direção
    /// </summary>
    public void RotateMirror(Vector3 newForward)
    {
        if (_useTransformForward)
        {
            transform.forward = newForward.normalized;
        }
        else
        {
            _mirrorNormal = newForward.normalized;
        }
    }

    /// <summary>
    /// Define a direção de reflexão do espelho (sem animação)
    /// </summary>
    public void SetReflectionDirection(ReflectionDirection direction)
    {
        _reflectionDirection = direction;
        _useCustomAngle = false;
        UpdateVisualOrientation();
        Debug.Log($"Mirror {gameObject.name} reflection direction set to: {direction}");
    }
    
    /// <summary>
    /// Define a direção de reflexão do espelho com animação
    /// </summary>
    public void SetReflectionDirectionAnimated(ReflectionDirection direction)
    {
        if (_isRotating) return; // Impede múltiplas rotações simultâneas
        
        if (_rotationCoroutine != null)
        {
            StopCoroutine(_rotationCoroutine);
        }
        
        _rotationCoroutine = StartCoroutine(RotateToDirectionCoroutine(direction));
    }

    /// <summary>
    /// Define um ângulo customizado de reflexão
    /// </summary>
    public void SetCustomReflectionAngle(float angle)
    {
        _customReflectionAngle = angle;
        _useCustomAngle = true;
        UpdateVisualOrientation();
        Debug.Log($"Mirror {gameObject.name} custom reflection angle set to: {angle}°");
    }

    /// <summary>
    /// Alterna entre as 4 direções disponíveis (ciclo completo)
    /// </summary>
    public void ToggleReflectionDirection()
    {
        if (_isRotating) return; // Impede rotação durante animação
        
        ReflectionDirection nextDirection;
        switch (_reflectionDirection)
        {
            case ReflectionDirection.Up:
                nextDirection = ReflectionDirection.Right;
                break;
            case ReflectionDirection.Right:
                nextDirection = ReflectionDirection.Down;
                break;
            case ReflectionDirection.Down:
                nextDirection = ReflectionDirection.Left;
                break;
            case ReflectionDirection.Left:
                nextDirection = ReflectionDirection.Up;
                break;
            default:
                nextDirection = ReflectionDirection.Right;
                break;
        }
        
        SetReflectionDirectionAnimated(nextDirection);
    }

    /// <summary>
    /// Corrotina para animação de rotação - rotaciona 90 graus no eixo Y
    /// </summary>
    private System.Collections.IEnumerator RotateToDirectionCoroutine(ReflectionDirection targetDirection)
    {
        _isRotating = true;
        
        if (transform.parent == null)
        {
            Debug.LogWarning($"Mirror {gameObject.name} não tem objeto pai para rotacionar");
            _isRotating = false;
            yield break;
        }
        
        // Pega a rotação atual do objeto pai
        Vector3 startRotation = transform.parent.localEulerAngles;
        
        // Rotaciona 90 graus no eixo Y (sempre incrementa)
        Vector3 targetRotation = startRotation + new Vector3(0, 90, 0);
        
        float elapsedTime = 0f;
        
        while (elapsedTime < _rotationAnimationDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / _rotationAnimationDuration;
            
            // Aplica a curva de animação
            float curveValue = _rotationCurve.Evaluate(normalizedTime);
            
            // Interpola entre rotação inicial e final
            Vector3 currentRotation = Vector3.Lerp(startRotation, targetRotation, curveValue);
            
            // Aplica a rotação ao objeto pai
            transform.parent.localRotation = Quaternion.Euler(currentRotation);
            
            yield return null;
        }
        
        // Garante a rotação final exata
        transform.parent.localRotation = Quaternion.Euler(targetRotation);
        
        // Atualiza a direção de reflexão
        _reflectionDirection = targetDirection;
        _useCustomAngle = false;
        
        // Atualiza referências para debug (não afeta a reflexão, que agora usa rotação do pai)
        UpdateReflectionDirectionFromParentRotation();
        
        _isRotating = false;
        
        Debug.Log($"Mirror {gameObject.name} rotated 90° in Y axis to direction: {targetDirection}");
    }
    
    /// <summary>
    /// Converte direção de reflexão para ângulo visual (não usado na animação, apenas para referência)
    /// </summary>
    private float GetVisualAngleFromDirection(ReflectionDirection direction)
    {
        switch (direction)
        {
            case ReflectionDirection.Up: return 0f;
            case ReflectionDirection.Right: return 90f;
            case ReflectionDirection.Down: return 180f;
            case ReflectionDirection.Left: return 270f;
            default: return 0f;
        }
    }

    /// <summary>
    /// Atualiza a orientação visual do objeto pai baseado na direção de reflexão
    /// </summary>
    private void UpdateVisualOrientation()
    {
        // Só atualiza o visual se tiver um objeto pai
        if (transform.parent == null)
        {
            Debug.LogWarning($"Mirror {gameObject.name} não tem objeto pai para rotacionar visualmente");
            return;
        }
        
        // Determina a rotação Y baseada na direção de reflexão
        float visualRotationY;
        
        if (_useCustomAngle)
        {
            // Para ângulos customizados, usar o próprio ângulo como rotação
            visualRotationY = _customReflectionAngle;
        }
        else
        {
            // Mapeamento específico para direções pré-definidas (4 direções)
            // Cada direção representa incrementos de 90 graus
            switch (_reflectionDirection)
            {
                case ReflectionDirection.Up:
                    visualRotationY = 0f; // Rot Y = 0 para cima (posição inicial)
                    break;
                case ReflectionDirection.Right:
                    visualRotationY = 90f; // Rot Y = 90 para direita
                    break;
                case ReflectionDirection.Down:
                    visualRotationY = 180f; // Rot Y = 180 para baixo
                    break;
                case ReflectionDirection.Left:
                    visualRotationY = 270f; // Rot Y = 270 para esquerda
                    break;
                default:
                    visualRotationY = 0f;
                    break;
            }
        }
        
        // Debug antes da aplicação
        Vector3 parentEulerBefore = transform.parent.localEulerAngles;
        Debug.Log($"Mirror {gameObject.name}: BEFORE - Parent local rotation: {parentEulerBefore}, Target Y: {visualRotationY}°");
        
        // Aplica a rotação apenas no eixo Y do objeto pai usando Quaternion (mais confiável)
        transform.parent.localRotation = Quaternion.Euler(0, visualRotationY, 0);
        
        // Debug depois da aplicação para ver o que realmente aconteceu
        Vector3 parentEulerAfter = transform.parent.localEulerAngles;
        Debug.Log($"Mirror {gameObject.name}: AFTER - Parent local rotation: {parentEulerAfter}, Expected Y: {visualRotationY}°, Actual Y: {parentEulerAfter.y}°");
    }

    /// <summary>
    /// Retorna a direção atual de reflexão
    /// </summary>
    public ReflectionDirection GetReflectionDirection()
    {
        return _reflectionDirection;
    }

    /// <summary>
    /// Retorna o ângulo atual de reflexão
    /// </summary>
    public float GetReflectionAngle()
    {
        return _useCustomAngle ? _customReflectionAngle : (float)_reflectionDirection;
    }

    /// <summary>
    /// Atualiza manualmente a orientação visual (útil se precisar reconfigurar)
    /// </summary>
    public void UpdateParentVisualOrientation()
    {
        UpdateVisualOrientation();
    }

    // Métodos para teste no inspector
    [ContextMenu("Set Direction: Up (0°)")]
    private void SetDirectionUp()
    {
        SetReflectionDirectionAnimated(ReflectionDirection.Up);
    }

    [ContextMenu("Set Direction: Right (90°)")]
    private void SetDirectionRight()
    {
        SetReflectionDirectionAnimated(ReflectionDirection.Right);
    }

    [ContextMenu("Set Direction: Down (180°)")]
    private void SetDirectionDown()
    {
        SetReflectionDirectionAnimated(ReflectionDirection.Down);
    }

    [ContextMenu("Set Direction: Left (-90°)")]
    private void SetDirectionLeft()
    {
        SetReflectionDirectionAnimated(ReflectionDirection.Left);
    }

    [ContextMenu("Toggle Direction (Animated)")]
    private void ToggleDirection()
    {
        ToggleReflectionDirection();
    }

    [ContextMenu("Update Parent Visual Orientation")]
    private void UpdateVisualOrientationMenu()
    {
        UpdateVisualOrientation();
    }

    // Métodos de debug para visualizar no Scene View
    private void OnDrawGizmos()
    {
        if (_showDebugRays)
        {
            Vector3 normal = _useTransformForward ? transform.forward : _mirrorNormal;
            
            // Desenha o normal do espelho
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, normal * _debugRayLength);
            
            // Desenha uma representação do espelho
            Gizmos.color = _isBeingHit ? Color.yellow : Color.white;
            Vector3 right = Vector3.Cross(normal, Vector3.up).normalized;
            if (right.magnitude < 0.1f) // Se normal está alinhado com up
                right = Vector3.Cross(normal, Vector3.forward).normalized;
            
            Vector3 up = Vector3.Cross(right, normal).normalized;
            
            // Desenha uma cruz representando a superfície do espelho
            float size = 0.5f;
            Gizmos.DrawLine(transform.position - right * size, transform.position + right * size);
            Gizmos.DrawLine(transform.position - up * size, transform.position + up * size);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Desenha informações mais detalhadas quando selecionado
        Vector3 normal = _useTransformForward ? transform.forward : _mirrorNormal;
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.1f);
        
        // Desenha a direção de reflexão atual
        float currentAngle = _useCustomAngle ? _customReflectionAngle : (float)_reflectionDirection;
        
        // Desenha seta vermelha mostrando a direção de entrada padrão (forward)
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.forward * _debugRayLength * 0.7f);
        
        // Desenha seta verde mostrando a direção de reflexão
        Gizmos.color = Color.green;
        Vector3 reflectedDir = Quaternion.AngleAxis(currentAngle, Vector3.up) * Vector3.forward;
        Gizmos.DrawRay(transform.position, reflectedDir * _debugRayLength * 0.7f);
        
        // Desenha texto com informações
        Gizmos.color = Color.white;
        #if UNITY_EDITOR
        UnityEditor.Handles.Label(transform.position + Vector3.up * 1f, 
            $"Direction: {_reflectionDirection}\nAngle: {currentAngle}°");
        #endif
    }
} 