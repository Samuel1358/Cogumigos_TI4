using TMPro;
using UnityEngine;

public class Finalsetter : MonoBehaviour {
    [SerializeField] private FinalDoor _leftDoor;
    [SerializeField] private FinalDoor _centerDoor;
    [SerializeField] private FinalDoor _rightDoor;
    [SerializeField] private CollectablePageDialog _trueFinl;
    [SerializeField] private CollectablePageDialog _fakeFinal;
    [SerializeField] private PersistenteCollectableDataSO[] CollectableDataSOs;
    [SerializeField] private TextMeshProUGUI _gameOverTextField;
    [SerializeField] private string _goodText;
    [SerializeField] private string _badText;
    private bool _goodFinal;

    private void Awake() {
        _fakeFinal.gameObject.SetActive(false);
        _trueFinl.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) {
        _goodFinal = true;
        foreach (PersistenteCollectableDataSO collectableBaseSO in CollectableDataSOs) {
            if (!collectableBaseSO.VerifyState()) {
                _goodFinal = false;
            }
            else {
                if (!collectableBaseSO.VerifyCollected()) {
                    _goodFinal = false;
                }
            }
        }
        if (_goodFinal) {
            _rightDoor.Activate();
            _leftDoor.Activate();
            _gameOverTextField.text = _goodText;
            _trueFinl.gameObject.SetActive(true);
        }
        else {
            _centerDoor.Activate();
            _gameOverTextField.text = _badText;
            _fakeFinal.gameObject.SetActive(true);
        }
    }
}
