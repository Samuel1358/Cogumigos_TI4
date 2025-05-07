using UnityEngine;

public class DeathFloor : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        RespawnController.OnPlayerRespawn.Invoke();
    }

    private void OnTriggerEnter(Collider other) {
        RespawnController.OnPlayerRespawn.Invoke();
    }
}
