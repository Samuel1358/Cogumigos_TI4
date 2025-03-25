using UnityEngine;

public class DeathFloor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        RespawnController.OnPlayerRespawn.Invoke();
    }
}
