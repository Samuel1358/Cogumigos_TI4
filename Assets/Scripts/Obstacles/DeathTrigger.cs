using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    private static bool isDeathSoundPlaying = false;

    private void OnTriggerEnter(Collider other) 
    {
        if ((Cheats.instance != null) ? !Cheats.instance.Imortal : true)
        {
            RespawnController.OnPlayerRespawn.Invoke();
            PlayDeathSound();
        }
    }

    private void PlayDeathSound()
    {
        if (!isDeathSoundPlaying)
        {
            isDeathSoundPlaying = true;
            AudioManager.Instance.PlaySFX("Death");
            Invoke(nameof(ResetDeathSoundFlag), 3.5f);
        }
    }

    private void ResetDeathSoundFlag()
    {
        isDeathSoundPlaying = false;
    }
}
