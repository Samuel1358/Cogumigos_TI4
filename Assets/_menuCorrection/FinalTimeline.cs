using System.Collections;
using UnityEngine;
using UnityEngine.Timeline;

public class FinalTimeline : MonoBehaviour
{
    [SerializeField] private GameObject timeline;
    private void OnTriggerEnter(Collider other) {
        if(other.transform.parent.TryGetComponent<Player>(out Player playi)) {
            playi.Input.DisableActionFor(playi.Input.PlayerActions.Move, 6f);
            playi.Input.DisableActionFor(playi.Input.PlayerActions.Look, 6f);
            playi.Input.DisableActionFor(playi.Input.PlayerActions.Interact, 6f);
            playi.Input.DisableActionFor(playi.Input.PlayerActions.Jump, 6f);
            playi.Input.DisableActionFor(playi.Input.PlayerActions.Pause, 6f);
            playi.Input.DisableActionFor(playi.Input.PlayerActions.SendCogu, 6f);
            timeline.SetActive(true);
            StartCoroutine(Rotin());
        }
    }
    private IEnumerator Rotin() {
        yield return new WaitForSeconds(6f);
        GameIniciator.Instance.GameOver();
    }
}
