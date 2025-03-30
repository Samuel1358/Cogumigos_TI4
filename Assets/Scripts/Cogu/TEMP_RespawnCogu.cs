using UnityEngine;

public class TEMP_RespawnCogu : MonoBehaviour
{
    [SerializeField] private GameObject prefeb;
    [SerializeField] private Cogu cogu;

    private void Start()
    {
        RespawnController.OnPlayerRespawn += Respawn;
    }

    private void Respawn()
    {
        if (cogu != null)
            Destroy(cogu.gameObject);

        cogu = Instantiate(prefeb, transform.position, Quaternion.identity).GetComponent<Cogu>();
    }
}
