using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        Player player = other.GetComponentInParent<Player>();
        if (player != null)
        {
            RespawnController.OnPlayerRespawn.Invoke();
            AudioManager.Instance.PlayDeathSound();
            UiInventory.Instance.UpdateCoguCountUI(GameManager.Instance.Player.CoguCast.CoguCount);
        }
    }
}
