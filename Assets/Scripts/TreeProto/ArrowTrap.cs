using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [Header("Arrow Settings")]
    public GameObject arrowPrefab;
    public float arrowSpeed = 10f;
    public float shootInterval = 2f;
    
    [Header("Shoot Points")]
    public Transform[] shootPoints;
    public Vector3 shootDirection = Vector3.right; // Direção no espaço do mundo
    
    [Header("Multi-Point Settings")]
    public bool useMultiplePoints = true; // Se true, alterna entre pontos. Se false, usa apenas o primeiro

    private float nextShootTime;
    private int currentShootPointIndex = 0;

    private ObjectPool _objectPool;

    private void Awake()
    {
        _objectPool = ObjectPool.CreateObjecPool(name + "_ObjectPool", new Vector3(1000, 1000, 1000));
        _objectPool.SetInstanceObject(arrowPrefab);
    }

    void Start()
    {
        // Se não há pontos configurados, usa o próprio transform
        if (shootPoints == null || shootPoints.Length == 0)
        {
            shootPoints = new Transform[] { transform };
        }
        
        // Remove nulls do array
        shootPoints = System.Array.FindAll(shootPoints, point => point != null);
        
        if (shootPoints.Length == 0)
        {
            Debug.LogWarning($"ArrowTrap {name}: No valid shoot points found! Using transform as default.");
            shootPoints = new Transform[] { transform };
        }
        
        nextShootTime = Time.time;
    }

    void Update()
    {
        if (Time.time >= nextShootTime)
        {
            ShootArrow();
            nextShootTime = Time.time + shootInterval;
        }
    }

    void ShootArrow()
    {
        // Seleciona o ponto atual
        Transform currentShootPoint = GetCurrentShootPoint();

        // Cria a flecha no ponto selecionado
        GameObject arrow = _objectPool.InstantiateObject(currentShootPoint.position, currentShootPoint.rotation, _objectPool.transform);
        Arrow arrowScript = arrow.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.Initialize(arrowSpeed, shootDirection, _objectPool);
        }
        
        // Avança para o próximo ponto (se usando múltiplos pontos)
        if (useMultiplePoints && shootPoints.Length > 1)
        {
            AdvanceToNextShootPoint();
        }
        
        //Debug.Log($"ArrowTrap {name}: Shot arrow from point {currentShootPointIndex + 1}/{shootPoints.Length}");
    }
    
    private Transform GetCurrentShootPoint()
    {
        // Garante que o índice está dentro dos limites
        if (currentShootPointIndex >= shootPoints.Length)
        {
            currentShootPointIndex = 0;
        }
        
        return shootPoints[currentShootPointIndex];
    }
    
    private void AdvanceToNextShootPoint()
    {
        currentShootPointIndex = (currentShootPointIndex + 1) % shootPoints.Length;
    }
    
    // Método público para resetar o índice (útil para testes)
    public void ResetToFirstShootPoint()
    {
        currentShootPointIndex = 0;
    }
    
    // Método público para definir um ponto específico
    public void SetCurrentShootPoint(int index)
    {
        if (index >= 0 && index < shootPoints.Length)
        {
            currentShootPointIndex = index;
        }
    }
} 