using UnityEngine;

public class DeathCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if ((Cheats.instance != null) ? !Cheats.instance.Imortal : true)
        {
            if (collision.transform.TryGetComponent(out Player player)) {
                RespawnController.OnPlayerRespawn.Invoke();
                AudioManager.Instance.PlayDeathSound();
                /*Debug.Log("=== NULL REFERANCE ===");
                Debug.Log("UI inventory: " + UiInventory.Instance);
                Debug.Log("GameManager: " + GameManager.Instance);
                Debug.Log("Player: " + GameManager.Instance.Player.gameObject);
                Debug.Log("CoguCast: " + GameManager.Instance.Player.CoguCast.);
                Debug.Log("----------------------");*/
                UiInventory.Instance.UpdateCoguCountUI(GameManager.Instance.Player.CoguCast.CoguCount);
            }
        }
    }
}
