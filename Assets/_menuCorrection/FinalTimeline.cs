using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Timeline;

public class FinalTimeline : MonoBehaviour
{
    [SerializeField] private GameObject timeline;
    [SerializeField] private Transform _target;
    [SerializeField] private CinemachineCamera _camera;
    private void OnTriggerEnter(Collider other) {
        if(other.transform.parent.TryGetComponent<Player>(out Player playi)) {
            playi.Input.DisableActionFor(playi.Input.PlayerActions.Move, 4.9f);
            playi.Input.DisableActionFor(playi.Input.PlayerActions.Look, 4.9f);
            playi.Input.DisableActionFor(playi.Input.PlayerActions.Interact, 4.9f);
            playi.Input.DisableActionFor(playi.Input.PlayerActions.Jump, 4.9f);
            playi.Input.DisableActionFor(playi.Input.PlayerActions.Pause, 4.9f);
            playi.Input.DisableActionFor(playi.Input.PlayerActions.SendCogu, 4.9f);
            _camera.Follow = _target;
            _camera.LookAt = _target;
            timeline.SetActive(true);
            StartCoroutine(Rotin());
        }
    }
    private IEnumerator Rotin() {
        yield return new WaitForSeconds(4.8f);
        GameIniciator.Instance.GameOver();
    }
}
