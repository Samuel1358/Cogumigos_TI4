using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [Header("Arrow Settings")]
    public GameObject arrowPrefab;
    public float arrowSpeed = 10f;
    public float shootInterval = 2f;
    public Transform shootPoint;
    public Vector3 shootDirection = Vector3.right; // Direção no espaço do mundo

    private float nextShootTime;

    void Start()
    {
        if (shootPoint == null)
        {
            shootPoint = transform;
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
        GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, shootPoint.rotation);
        Arrow arrowScript = arrow.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.Initialize(arrowSpeed, shootDirection);
        }
    }
} 