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
                UiInventory.Instance.UpdateCoguCountUI(GameManager.Instance.Player.CoguCast.CoguCount);
            }
        }
    }
}
