using UnityEngine;

public class Finalsetter : MonoBehaviour
{
    [SerializeField] private FinalDoor _leftDoor;
    [SerializeField] private FinalDoor _centerDoor;
    [SerializeField] private FinalDoor _rightDoor;
    [SerializeField] private CollectablePageDialog _trueFinl;
    [SerializeField] private CollectablePageDialog _fakeFinal;
    [SerializeField] private PersistenteCollectableDataSO[] CollectableDataSOs;
    private bool _goodFinal;

    private void OnTriggerEnter(Collider other) {
        _goodFinal = true;
        foreach(PersistenteCollectableDataSO collectableBaseSO in CollectableDataSOs) {
            if (!collectableBaseSO.VerifyCollected()) {
                _goodFinal = false;
            }
        }
        if (_goodFinal) {
            _rightDoor.Activate();
            _leftDoor.Activate();
            _trueFinl.gameObject.SetActive(true);
        }
        else {
            _centerDoor.Activate();
            _fakeFinal.gameObject.SetActive(true);
        }
    }
}
