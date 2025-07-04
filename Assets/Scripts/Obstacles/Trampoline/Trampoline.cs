using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Trampoline : MonoBehaviour
{
    [Header("Bounce Settings")]
    [SerializeField] private float bounceForce = 30f;

    [Space]
    [SerializeField] private UnityEvent _onBounce;

    private void OnCollisionEnter(Collision collision)
    {
        Player player = collision.collider.GetComponentInParent<Player>();
        if (player == null) return;

        player.ChangeToTrampolineJumpState(bounceForce);
        _onBounce.Invoke();
    
        GameIniciator.Instance.AudioManagerInstance.PlaySFX("TrampolineBounce2");
    }
}
