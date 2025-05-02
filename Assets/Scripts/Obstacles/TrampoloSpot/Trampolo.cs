using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Trampoline : MonoBehaviour
{
    [Header("Bounce Settings")]
    [SerializeField] private float bounceForce = 30f;             
    [SerializeField] private float bounceCooldown = 0.2f;      
    [SerializeField] private float jumpDuration = 1f;
    [SerializeField] private bool useLocalUpDirection = true; // Usar direção local do trampolim ou global

    [Header("Effects")]
    [SerializeField] private AudioClip bounceSound;
    [SerializeField] private ParticleSystem bounceEffect;

    private float lastBounceTime;               
    private AudioSource audioSource;
    private Collider trampolineCollider;

    private void Awake()
    {
        trampolineCollider = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();
        
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Garantir que o collider está configurado corretamente
        if (trampolineCollider != null)
        {
            trampolineCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        if (Time.time < lastBounceTime + bounceCooldown) return;

        Player player = other.GetComponentInParent<Player>();
        if (player == null) return;

        // Mudar para o estado de pulo do trampolim e passar os parâmetros
        player.ChangeToTrampolineJumpState(bounceForce, jumpDuration);
        AudioManager.Instance.PlaySFX("Teste");

        // Tocar som e efeito
        if (bounceSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(bounceSound);
        }

        if (bounceEffect != null)
        {
            bounceEffect.Play();
        }

        lastBounceTime = Time.time;
    }

    private void OnDrawGizmos()
    {
        // Desenhar gizmo para visualizar a direção do pulo
        Gizmos.color = Color.green;
        Vector3 direction = useLocalUpDirection ? transform.up : Vector3.up;
        Gizmos.DrawRay(transform.position, direction * 2f);
    }
}
