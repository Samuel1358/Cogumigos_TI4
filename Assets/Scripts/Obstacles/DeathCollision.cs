using UnityEngine;

public class DeathCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if ((GameIniciator.Instance.CheatsInstance != null) ? !GameIniciator.Instance.CheatsInstance.Imortal : true)
        {
            if (collision.transform.TryGetComponent(out Player player)) {
                GameIniciator.Instance.RespawnControllerInstance.OnPlayerRespawn.Invoke();
                GameIniciator.Instance.AudioManagerInstance.PlayDeathSound();
                //GameIniciator.Instance.CanvasIniciatorInstance.InventoryCanvas.UpdateCoguCountUI(GameIniciator.Instance.GameManagerInstance.Player.CoguCast.CoguCount);
            }
        }
    }
}
