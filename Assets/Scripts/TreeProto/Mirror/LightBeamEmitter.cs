using UnityEngine;

public class LightBeamEmitter : MonoBehaviour
{
    [Header("Beam Settings")]
    [SerializeField] private Transform _beamStartPoint;
    [SerializeField] private float _maxBeamDistance = 50f;
    [SerializeField] private LayerMask _reflectableLayers = -1;
    [SerializeField] private LayerMask _obstacleLayers = -1;
    [SerializeField] private bool _ignoreLayers = true; // Para teste - ignora layers e considera qualquer MirrorReflector
    
    [Header("Visual Settings")]
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Material _beamMaterial;
    [SerializeField] private float _beamWidth = 0.1f;
    [SerializeField] private Color _beamColor = Color.red;
    
    [Header("References")]
    [SerializeField] private ActivateSwitch _activateSwitch;
    
    private bool _isBeamActive = false;
    private LightBeamTarget _lastHitTarget = null;

    private void Start()
    {
        // Setup LineRenderer
        if (_lineRenderer == null)
            _lineRenderer = GetComponent<LineRenderer>();
            
        if (_lineRenderer == null)
        {
            _lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        
        SetupLineRenderer();
        
        // Setup beam start point if not assigned
        if (_beamStartPoint == null)
            _beamStartPoint = transform;
            
        // Subscribe to switch events
        if (_activateSwitch != null)
        {
            _activateSwitch.onActivate.AddListener(ActivateBeam);
            _activateSwitch.onDeactivate.AddListener(DeactivateBeam);
        }
        
        // Initial state
        _isBeamActive = _activateSwitch != null ? _activateSwitch.isActivated : false;
        UpdateBeam();
    }

    private void OnDestroy()
    {
        // Unsubscribe from switch events
        if (_activateSwitch != null)
        {
            _activateSwitch.onActivate.RemoveListener(ActivateBeam);
            _activateSwitch.onDeactivate.RemoveListener(DeactivateBeam);
        }
    }

    private void Update()
    {
        if (_isBeamActive)
        {
            UpdateBeam();
        }
    }

    private void SetupLineRenderer()
    {
        _lineRenderer.material = _beamMaterial;
        _lineRenderer.startColor = _beamColor;
        _lineRenderer.endColor = _beamColor;
        _lineRenderer.startWidth = _beamWidth;
        _lineRenderer.endWidth = _beamWidth;
        _lineRenderer.useWorldSpace = true;
        _lineRenderer.enabled = false;
    }

    public void ActivateBeam()
    {
        _isBeamActive = true;
        _lineRenderer.enabled = true;
        Debug.Log($"Light beam activated on {gameObject.name}");
        UpdateBeam();
    }

    public void DeactivateBeam()
    {
        _isBeamActive = false;
        _lineRenderer.enabled = false;
        
        // Notifica o último alvo que parou de ser atingido
        if (_lastHitTarget != null)
        {
            _lastHitTarget.OnBeamMiss();
            _lastHitTarget = null;
        }
        
        Debug.Log($"Light beam deactivated on {gameObject.name}");
    }

    private void UpdateBeam()
    {
        if (!_isBeamActive || _lineRenderer == null)
            return;

        Vector3 startPos = _beamStartPoint.position;
        Vector3 direction = _beamStartPoint.forward;
        
        // Calculate beam path with reflections
        CalculateBeamPath(startPos, direction);
    }

    private void CalculateBeamPath(Vector3 startPos, Vector3 direction)
    {
        var beamPoints = new System.Collections.Generic.List<Vector3>();
        beamPoints.Add(startPos);

        Vector3 currentPos = startPos;
        Vector3 currentDirection = direction;
        float remainingDistance = _maxBeamDistance;
        int maxReflections = 10; // Prevent infinite loops
        int reflectionCount = 0;

        while (remainingDistance > 0 && reflectionCount < maxReflections)
        {
            Ray ray = new Ray(currentPos, currentDirection);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, remainingDistance, _reflectableLayers | _obstacleLayers))
            {
                beamPoints.Add(hit.point);

                                // Check if hit a reflectable object (mirror)
                MirrorReflector mirror = hit.collider.GetComponent<MirrorReflector>();
                if (mirror == null)
                {
                    // Se não encontrou no objeto atingido, procura nos pais
                    mirror = hit.collider.GetComponentInParent<MirrorReflector>();
                }
                
                // Debug log para verificar o que está acontecendo
                Debug.Log($"Hit object: {hit.collider.name}, Has Mirror: {mirror != null}, Layer: {hit.collider.gameObject.layer}, LayerMask includes: {(_reflectableLayers.value & (1 << hit.collider.gameObject.layer)) != 0}");
                
                // Se tem o componente MirrorReflector, considera como espelho (independente da layer para simplificar)
                bool isReflectableLayer = (_reflectableLayers.value & (1 << hit.collider.gameObject.layer)) != 0;
                
                if (mirror != null && (_ignoreLayers || isReflectableLayer || _reflectableLayers == -1))
                {
                    Debug.Log($"Reflecting beam on mirror: {hit.collider.name}");
                    
                    // Get reflected direction from mirror
                    Vector3 reflectedDirection = mirror.GetReflectedDirection(currentDirection, hit.normal);
                    
                    Debug.Log($"Incoming direction: {currentDirection}, Reflected direction: {reflectedDirection}");
                    
                    // Continue beam from reflection point
                    currentPos = hit.point + reflectedDirection * 0.01f; // Increased offset to avoid self-collision
                    currentDirection = reflectedDirection;
                    remainingDistance -= hit.distance;
                    reflectionCount++;
                }
                else
                {
                    // Hit an obstacle or non-reflectable surface
                    // Check if it's a target (first on hit object, then on parents)
                    LightBeamTarget target = hit.collider.GetComponent<LightBeamTarget>();
                    if (target == null)
                    {
                        // Se não encontrou no objeto atingido, procura nos pais
                        target = hit.collider.GetComponentInParent<LightBeamTarget>();
                    }
                    
                    if (target != null)
                    {
                        Debug.Log($"Hit target: {target.name} (through {hit.collider.name})");
                        
                        // Se mudou de alvo, notifica o anterior que parou de ser atingido
                        if (_lastHitTarget != null && _lastHitTarget != target)
                        {
                            _lastHitTarget.OnBeamMiss();
                        }
                        
                        target.OnBeamHit();
                        _lastHitTarget = target;
                    }
                    else
                    {
                        Debug.Log($"Hit obstacle: {hit.collider.name} (no target found)");
                        
                        // Se não atingiu um alvo, notifica o último alvo que parou de ser atingido
                        if (_lastHitTarget != null)
                        {
                            _lastHitTarget.OnBeamMiss();
                            _lastHitTarget = null;
                        }
                    }
                    break;
                }
            }
            else
            {
                // No hit, beam goes to max distance
                beamPoints.Add(currentPos + currentDirection * remainingDistance);
                
                // Se não atingiu nada, notifica o último alvo que parou de ser atingido
                if (_lastHitTarget != null)
                {
                    _lastHitTarget.OnBeamMiss();
                    _lastHitTarget = null;
                }
                break;
            }
        }

        // Update LineRenderer with all points
        _lineRenderer.positionCount = beamPoints.Count;
        _lineRenderer.SetPositions(beamPoints.ToArray());
    }

    // Public method to manually trigger beam calculation (useful for editor testing)
    [ContextMenu("Update Beam")]
    public void ForceUpdateBeam()
    {
        if (_isBeamActive)
            UpdateBeam();
    }
} 