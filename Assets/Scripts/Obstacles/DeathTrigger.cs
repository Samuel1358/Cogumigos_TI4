using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        if (other.transform.TryGetComponent(out Player player))
        {
            RespawnController.OnPlayerRespawn.Invoke();
            AudioManager.Instance.PlayDeathSound();
            UiInventory.Instance.UpdateCoguCountUI(GameManager.Instance.Player.CoguCast.CoguCount);
        }
    }
}
