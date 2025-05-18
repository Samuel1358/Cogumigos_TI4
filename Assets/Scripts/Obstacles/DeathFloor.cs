using UnityEngine;

public class DeathFloor : MonoBehaviour
{
    [SerializeField] private LayerMask includeLayers = 1 << 3; // Layer 3 is typically the Player layer
    private static bool isDeathSoundPlaying = false;

    private void OnCollisionEnter(Collision collision)
    {
        if ((includeLayers & (1 << collision.gameObject.layer)) == 0) return;
        
        if (!Cheats.instance.imortal)
        {
            RespawnController.OnPlayerRespawn.Invoke();
            PlayDeathSound();
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if ((includeLayers & (1 << other.gameObject.layer)) == 0) return;

        RespawnController.OnPlayerRespawn.Invoke();
        PlayDeathSound();
    }

    private void PlayDeathSound()
    {
        if (!isDeathSoundPlaying)
        {
            isDeathSoundPlaying = true;
            AudioManager.Instance.PlaySFX("Death");
            // Reset the flag after the sound duration (assuming it's around 2 seconds)
            Invoke(nameof(ResetDeathSoundFlag), 3.5f);
        }
    }

    private void ResetDeathSoundFlag()
    {
        isDeathSoundPlaying = false;
    }
}
