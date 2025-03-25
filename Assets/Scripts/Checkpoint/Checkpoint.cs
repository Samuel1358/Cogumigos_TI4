using System;
using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private int _coguGain;
    [SerializeField] private bool _canActive = true;
    private void OnTriggerEnter(Collider other) {
        if (other.transform.parent.TryGetComponent<Player>(out Player player)) {
            if (_canActive) {
                player.SetCheckpoint(_coguGain);
                RespawnController.Instance.SetActiveCheckPoint(this);
                RespawnController.OnPlayerChangeCheckPoint.Invoke(this);
                _canActive = false;
            }
        }
    }
}
