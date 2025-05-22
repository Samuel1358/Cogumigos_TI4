using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Trampoline : MonoBehaviour
{
    [Header("Bounce Settings")]
    [SerializeField] private float bounceForce = 30f;

    private void OnCollisionEnter(Collision collision)
    {
        Player player = collision.collider.GetComponentInParent<Player>();
        if (player == null) return;

        player.ChangeToTrampolineJumpState(bounceForce);
    
        AudioManager.Instance.PlaySFX("TrampolineBounce2");
    }
}
