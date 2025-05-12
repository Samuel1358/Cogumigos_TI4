using UnityEngine;

public class DeathFloor : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (!Cheats.instance.imortal)
        {
            RespawnController.OnPlayerRespawn.Invoke();
            AudioManager.Instance.PlaySFX("Death");
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        //if (!Cheats.instance.imortal)
        //{
            RespawnController.OnPlayerRespawn.Invoke();
            AudioManager.Instance.PlaySFX("Death");
        //}
    }
}
