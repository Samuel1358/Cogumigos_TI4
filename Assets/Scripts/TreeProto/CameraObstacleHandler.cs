using UnityEngine;
using Unity.Cinemachine;

[RequireComponent(typeof(CinemachineCamera))]
public class CameraObstacleHandler : MonoBehaviour
{
    [Header("Obstacle Detection")]
    [SerializeField] private LayerMask obstacleLayers = ~0; // Todas as layers por padrão
    [SerializeField, Min(0.1f)] private float raycastRadius = 0.2f; // Espessura do Raycast
    [SerializeField, Min(0f)] private float collisionOffset = 0.3f; // Margem de segurança
    [SerializeField, Min(1f)] private float approachSpeed = 5f; // Velocidade de ajuste

    [Header("Distance Settings")]
    [SerializeField, Min(1f)] private float minDistance = 2f; // Distância mínima
    [SerializeField, Min(5f)] private float maxDistance = 10f; // Distância máxima

    // Referências
    [SerializeField] private CinemachineCamera _cmCamera;
    [SerializeField] private CinemachineOrbitalFollow _orbitalFollow;
    [SerializeField] private Transform _player;

    private void Awake()
    {
        _cmCamera = GetComponent<CinemachineCamera>();
        _orbitalFollow = _cmCamera.GetComponent<CinemachineOrbitalFollow>();
        _player = _cmCamera.Follow;

        if (_orbitalFollow == null || _player == null)
        {
            Debug.LogError("Componentes essenciais não encontrados!", this);
            enabled = false;
        }
    }

    private void LateUpdate()
    {
        Vector3 cameraPos = transform.position;
        Vector3 playerPos = _player.position;
        Vector3 direction = (playerPos - cameraPos).normalized;
        float currentDistance = Vector3.Distance(cameraPos, playerPos);

        // Verifica se há um obstáculo entre a câmera e o jogador
        if (Physics.SphereCast(
            cameraPos,
            raycastRadius,
            direction,
            out RaycastHit hit,
            currentDistance,
            obstacleLayers,
            QueryTriggerInteraction.Ignore
        ))
        {
            // Se bateu em algo que NÃO é o jogador, aproxima a câmera
            if (hit.transform != _player)
            {
                Debug.Log($"Obstáculo detectado: {hit.transform.name}");
                float desiredDistance = hit.distance - collisionOffset;
                float newRadius = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
                
                // Ajuste suave da distância
                _orbitalFollow.Radius = Mathf.Lerp(
                    _orbitalFollow.Radius,
                    newRadius,
                    approachSpeed * Time.deltaTime
                );
            }
        }
        else
        {
            // Se não há obstáculos, volta à distância padrão (máxima)
            _orbitalFollow.Radius = Mathf.Lerp(
                _orbitalFollow.Radius,
                maxDistance,
                approachSpeed * Time.deltaTime
            );
        }
    }

    // Debug: Desenha o Raycast no Editor
    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying || _cmCamera == null || _player == null)
            return;

        Gizmos.color = Color.red;
        Vector3 direction = (_player.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, _player.position);
        Gizmos.DrawRay(transform.position, direction * distance);
        Gizmos.DrawWireSphere(transform.position + direction * distance, raycastRadius);
    }
}