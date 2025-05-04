using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Trampoline : MonoBehaviour
{
    [Header("Bounce Settings")]
    [SerializeField] private float bounceForce = 30f;             
    [SerializeField] private LayerMask includeLayers; // Layers que ativam o trampolim

    private Collider trampolineCollider;

    private void Awake()
    {
        trampolineCollider = GetComponent<Collider>();
        if (trampolineCollider != null)
        {
            trampolineCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((includeLayers & (1 << other.gameObject.layer)) == 0) return;

        Player player = other.GetComponentInParent<Player>();
        if (player == null) return;

        // Mudar para o estado de pulo do trampolim e passar apenas a força
        player.ChangeToTrampolineJumpState(bounceForce);
    }
}
