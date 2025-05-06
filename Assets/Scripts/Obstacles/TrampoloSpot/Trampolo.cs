using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Trampoline : MonoBehaviour
{
    [Header("Bounce Settings")]
    [SerializeField] private float bounceForce = 30f;             
    [SerializeField] private LayerMask includeLayers;

    private Collider trampolineCollider;

    private void Awake()
    {
        trampolineCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((includeLayers & (1 << other.gameObject.layer)) == 0) return;

        Player player = other.GetComponentInParent<Player>();
        if (player == null) return;

        player.ChangeToTrampolineJumpState(bounceForce);
    }
}
