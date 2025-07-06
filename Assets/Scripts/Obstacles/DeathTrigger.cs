using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        Player player = other.GetComponentInParent<Player>();
        if (player != null)
        {
            if(GameIniciator.Instance.AudioManagerInstance == null) {
				Debug.Log("Audio manager nulo");
			}
            if(InGameMenuInitiator.Instance.InventoryCanvas == null) {
				Debug.Log("Ui inventory nulo");
			}
            if(GameIniciator.Instance.GameManagerInstance == null) {
                Debug.Log("Game manager Nulo");
            }
            GameIniciator.Instance.RespawnControllerInstance.OnPlayerRespawn.Invoke();
            GameIniciator.Instance.AudioManagerInstance.PlayDeathSound();
        }
    }
}
