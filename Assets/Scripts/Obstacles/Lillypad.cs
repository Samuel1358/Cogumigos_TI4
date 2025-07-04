using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lillypad : MonoBehaviour
{
    [Header("Water Movement")]
    [SerializeField] public float xAmplitude = 0.5f;          
    [SerializeField] public float zAmplitude = 0.3f;          
    [SerializeField] public float xSpeed = 1.0f;              
    [SerializeField] public float zSpeed = 0.8f;              
    [SerializeField] public float rotationAmplitude = 2.0f;   
    [SerializeField] public float rotationSpeed = 0.5f;       
    
    [Header("Waypoint Control")]
    [SerializeField] private bool isControlledByWaypoints = false;
    
    [Header("Player Detection")]
    [SerializeField] private LayerMask playerLayer = 1;
    
    private List<Transform> waypointPath = new List<Transform>();
    private int currentWaypointIndex = 0;
    private bool isMovingToWaypoint = false;
    private float waypointMovementSpeed = 0f;
    private float sinkSpeed = 2f;
    private float sinkDepth = 3f;
    private float disappearDelay = 1f;
    private bool isSinking = false;
    private Vector3 initialSinkPosition;
    private Vector3 waypointMovementThisFrame = Vector3.zero;
    
    // Water movement variables
    private Vector3 initialPosition;
    private Vector3 initialRotation;
    private Vector3 waterMovementThisFrame = Vector3.zero;
    private float timeOffset;
    private Transform playerOnPlatform;

    private ObjectPool _pool;
    
    // Events
    public System.Action<Lillypad> OnLillypadDestroyed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Store initial position and rotation
        initialPosition = transform.position;
        initialRotation = transform.eulerAngles;
        
        timeOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    // Update is called once per frame
    void Update()
    {
        waterMovementThisFrame = Vector3.zero;
        waypointMovementThisFrame = Vector3.zero;
        
        if (isSinking)
        {
            HandleSinking();
            return;
        }
        
        if (isControlledByWaypoints)
        {
            HandleWaypointMovement();
            MovePlayerWithWaypoints();
        }
        else
        {
            HandleWaterMovement();
            MovePlayerWithWater();
        }
        
        HandleRotation();
    }

    public void Initialize(ObjectPool objectPool)
    {
        _pool = objectPool;

        initialPosition = transform.position;
        initialRotation = transform.eulerAngles;

        timeOffset = Random.Range(0f, Mathf.PI * 2f);

        // properties
        currentWaypointIndex = 0;
        isMovingToWaypoint = false;
        waypointMovementSpeed = 0f;
        sinkSpeed = 2f;
        sinkDepth = 3f;
        disappearDelay = 1f;
        isSinking = false;
        waypointMovementThisFrame = Vector3.zero;
        waterMovementThisFrame = Vector3.zero;
    }

    private void HandleSinking()
    {
        Vector3 sinkMovement = Vector3.down * sinkSpeed * Time.deltaTime;
        Vector3 newPosition = transform.position + sinkMovement;
        
        if (newPosition.y <= initialSinkPosition.y - sinkDepth)
        {
            OnLillypadDestroyed?.Invoke(this);
            _pool.ReturnObject(gameObject);
            return;
        }
        
        transform.position = newPosition;
        
        if (playerOnPlatform != null)
        {
            playerOnPlatform.position += sinkMovement;
        }
    }

    private void HandleWaterMovement()
    {
        float time = Time.time + timeOffset;
        float xMovement = Mathf.Sin(time * xSpeed) * xAmplitude;
        float zMovement = Mathf.Sin(time * zSpeed * 0.7f) * zAmplitude;
        Vector3 targetWaterPosition = initialPosition + new Vector3(xMovement, 0, zMovement);
        waterMovementThisFrame = targetWaterPosition - transform.position;
        transform.position += waterMovementThisFrame;
    }

    private void HandleWaypointMovement()
    {
        if (isMovingToWaypoint)
        {
            waypointMovementThisFrame = MoveTowardsCurrentWaypoint();
            transform.position += waypointMovementThisFrame;
            // Atualiza initialPosition apenas se não chegou a um waypoint
            if (waypointMovementThisFrame != Vector3.zero)
            {
                initialPosition += waypointMovementThisFrame;
            }
        }
    }

    private void HandleRotation()
    {
        float time = Time.time + timeOffset;
        float yRotation = Mathf.Sin(time * rotationSpeed) * rotationAmplitude;
        float zRotation = Mathf.Cos(time * rotationSpeed * 0.8f) * rotationAmplitude * 0.5f;
        Vector3 newRotation = initialRotation + new Vector3(0, yRotation, zRotation);
        transform.eulerAngles = newRotation;
    }

    private void MovePlayerWithWater()
    {
        if (playerOnPlatform != null)
        {
            playerOnPlatform.position += waterMovementThisFrame;
        }
    }

    private void MovePlayerWithWaypoints()
    {
        if (playerOnPlatform != null)
        {
            playerOnPlatform.position += waypointMovementThisFrame;
        }
        else
        {
            // Só loga se o player está próximo desta Lillypad
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            
        }
    }

    private Vector3 MoveTowardsCurrentWaypoint()
    {
        if (currentWaypointIndex >= waypointPath.Count)
        {
            return Vector3.zero;
        }

        Transform targetWaypoint = waypointPath[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        Vector3 movement = direction * waypointMovementSpeed * Time.deltaTime;
        
        float distanceToWaypoint = Vector3.Distance(transform.position, targetWaypoint.position);
        bool reachedWaypoint = distanceToWaypoint <= 0.1f || movement.magnitude >= distanceToWaypoint;
        
        if (reachedWaypoint)
        {
            // Move para a posição exata do waypoint
            Vector3 actualMovement = targetWaypoint.position - transform.position;
            transform.position = targetWaypoint.position;
            initialPosition = targetWaypoint.position;
            currentWaypointIndex++;
            
            if (currentWaypointIndex >= waypointPath.Count)
            {
                ReachedFinalWaypoint();
                return Vector3.zero;
            }
            
            // Retorna o movimento real feito para este frame
            return actualMovement;
        }
        
        return movement;
    }

    private void ReachedFinalWaypoint()
    {
        isControlledByWaypoints = false;
        isMovingToWaypoint = false;
        initialSinkPosition = transform.position;
        initialPosition = transform.position;
        StartCoroutine(StartSinkingAfterDelay());
    }

    private IEnumerator StartSinkingAfterDelay()
    {
        yield return new WaitForSeconds(disappearDelay);
        isSinking = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsInLayerMask(collision.gameObject, playerLayer))
        {
            playerOnPlatform = collision.transform;
            AudioManager.Instance.PlaySFX(SoundEffectNames.LILLYPAD_CAINDO);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (IsInLayerMask(collision.gameObject, playerLayer))
        {
            StartCoroutine(DelayedPlayerRemoval(collision.transform));
        }
    }

    private IEnumerator DelayedPlayerRemoval(Transform playerTransform)
    {
        yield return new WaitForSeconds(0.1f);
        
        if (playerOnPlatform == playerTransform)
        {
            playerOnPlatform = null;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (IsInLayerMask(collision.gameObject, playerLayer))
        {
            if (playerOnPlatform != collision.transform)
            {
                playerOnPlatform = collision.transform;
            }
        }
    }

    private bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return (layerMask.value & (1 << obj.layer)) > 0;
    }

    // Public methods for waypoint control
    public void SetWaypointControl(bool controlled, List<Transform> path, float movementSpeed, float sSpeed, float sDepth, float delay)
    {
        isControlledByWaypoints = controlled;
        
        if (controlled && path != null && path.Count > 0)
        {
            waypointPath = new List<Transform>(path);
            waypointMovementSpeed = movementSpeed;
            sinkSpeed = sSpeed;
            sinkDepth = sDepth;
            disappearDelay = delay;
            currentWaypointIndex = 1;
            isMovingToWaypoint = true;
        }
        else
        {
            isMovingToWaypoint = false;
            waypointPath.Clear();
        }
    }
}
