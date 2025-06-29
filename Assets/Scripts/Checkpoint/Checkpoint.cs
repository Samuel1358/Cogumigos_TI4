using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private bool _canActive = true;
    [SerializeField] private UnityEvent _onSetCheckpoint;

    private void OnTriggerEnter(Collider other) {
        if (other.transform.parent.TryGetComponent<Player>(out Player player)) {
            if (_canActive) {
                RespawnController.Instance.SetActiveCheckPoint(this);
                RespawnController.OnPlayerChangeCheckPoint.Invoke(this);
                _onSetCheckpoint.Invoke();
                _canActive = false;
            }
        }
    }
}
