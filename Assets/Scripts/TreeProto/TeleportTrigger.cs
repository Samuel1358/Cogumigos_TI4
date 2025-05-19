using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    [Header("Teleport Settings")]
    [SerializeField] private Transform pointA; // Ponto de origem (opcional)
    [SerializeField] private Transform pointB; // Ponto de destino
    [SerializeField] private float teleportDelay = 0.5f; // Pequeno atraso antes do teleporte
    [SerializeField] private ParticleSystem teleportEffect; // Efeito visual (opcional)
    [SerializeField] private AudioClip teleportSound; // Som (opcional)

    private bool isTeleporting = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isTeleporting && other.CompareTag("Player"))
        {
            StartCoroutine(TeleportPlayer(other.transform));
        }
    }

    private System.Collections.IEnumerator TeleportPlayer(Transform playerTransform)
    {
        isTeleporting = true;
        
        // Opcional: Tocar efeito/som no ponto A
        if (teleportEffect != null) Instantiate(teleportEffect, playerTransform.position, Quaternion.identity);
        if (teleportSound != null) AudioSource.PlayClipAtPoint(teleportSound, playerTransform.position);

        // Pequeno delay antes do teleporte
        yield return new WaitForSeconds(teleportDelay);

        // Teleporta o jogador
        playerTransform.position = pointB.position;

        // Opcional: Tocar efeito/som no ponto B
        if (teleportEffect != null) Instantiate(teleportEffect, pointB.position, Quaternion.identity);
        if (teleportSound != null) AudioSource.PlayClipAtPoint(teleportSound, pointB.position);

        isTeleporting = false;
    }

    // Desenha gizmos no editor para visualização
    private void OnDrawGizmos()
    {
        if (pointB != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(pointB.position, 0.5f);
            Gizmos.DrawLine(transform.position, pointB.position);
        }
    }
}