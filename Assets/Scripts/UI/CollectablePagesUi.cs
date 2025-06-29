using UnityEngine;

public class CollectablePagesUi : MonoBehaviour
{
    private PersistenteCollectableDataSO[] CollectableDataSOs;
    private void Start() {
        CollectableDataSOs = Resources.LoadAll<PersistenteCollectableDataSO>("Collectables");
    }
}
