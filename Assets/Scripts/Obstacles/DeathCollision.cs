using UnityEngine;

public class DeathCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if ((Cheats.instance != null) ? !Cheats.instance.Imortal : true)
        {
            RespawnController.OnPlayerRespawn.Invoke();
            AudioManager.Instance.PlayDeathSound();
        }
    }
}
