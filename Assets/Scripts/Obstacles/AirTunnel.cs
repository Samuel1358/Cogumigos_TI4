using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AirTunnel : MonoBehaviour {
    
    [Header("Air Tunnel Settings")]
    [SerializeField] private Transform destinationPoint;
    [SerializeField] private float movementDuration = 2f;
    [SerializeField] private Ease movementEase = Ease.InOutQuad;
    
    [Header("Waypoint System")]
    [SerializeField] private List<Transform> waypoints = new List<Transform>();
    [SerializeField] private float waypointMovementDuration = 1f;
    
    [Header("Player Detection")]
    [SerializeField] private LayerMask playerLayer = 1 << 7; // Layer 7 = TriggerCheck
    
    private Player player;
    private bool isPlayerInTunnel = false;
    private Tweener movementTween;
    private int currentWaypointIndex = 0;
    
    private void OnTriggerEnter(Collider other) {
        Debug.Log("smth entered the air tunnel");
        Debug.Log(isPlayerInTunnel);
        Debug.Log($"Object: {other.gameObject.name}, Layer: {other.gameObject.layer}, LayerMask: {playerLayer.value}");
        
        if (IsInLayerMask(other.gameObject, playerLayer) && !isPlayerInTunnel) {
            // Pega o Player do pai do TriggerCheck
            player = other.GetComponentInParent<Player>();
            Debug.Log(player);
            if (player != null) {
                Debug.Log("Player entered the air tunnel");
                StartAirTunnelSequence();
            }
        }
    }
    
    private bool IsInLayerMask(GameObject obj, LayerMask layerMask) {
        return (layerMask.value & (1 << obj.layer)) > 0;
    }
    
    private void StartAirTunnelSequence() {
        isPlayerInTunnel = true;
        currentWaypointIndex = 0;
        
        // Muda o estado do jogador para Glide
        player.SetGlide(true);
        
        // Inicia o movimento pelos waypoints
        MoveToNextWaypoint();
    }
    
    private void MoveToNextWaypoint() {
        if (currentWaypointIndex >= waypoints.Count) {
            // Chegou ao final, move para o destino final
            MoveToDestination();
            return;
        }
        
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        
        Debug.Log($"Moving to waypoint {currentWaypointIndex}: {targetWaypoint.name}");
        
        movementTween = player.transform.DOMove(targetWaypoint.position, waypointMovementDuration)
        .SetEase(movementEase)
        .OnComplete(() => {
            currentWaypointIndex++;
            MoveToNextWaypoint();
        });
    }
    
    private void MoveToDestination() {
        Debug.Log("Moving to final destination");
        
        movementTween = player.transform.DOMove(destinationPoint.position, movementDuration)
        .SetEase(movementEase)
        .OnComplete(() => {
            // Muda o estado para Falling
            player.SetGlide(false);
            
            // Reseta a flag
            isPlayerInTunnel = false;
            
            Debug.Log("Air tunnel sequence completed");
        });
    }
    
    private void OnDestroy() {
        // Cancela o tween se o objeto for destruído
        if (movementTween != null && movementTween.IsActive()) {
            movementTween.Kill();
        }
    }

    private void OnDrawGizmos()
    {
        Vector3? oldVec = null;
        foreach (var pathPoint in waypoints)
        {
            if (pathPoint != null)
                if (oldVec == null)
                {
                    oldVec = pathPoint.position;
                }
                else
                {
                    Gizmos.DrawLine(oldVec.Value, pathPoint.position);
                    oldVec = pathPoint.position;
                }
        }
    }
}
