using UnityEngine;

public class PlayerAnimationTriggerEvent : MonoBehaviour
{
    private Player _player;
    private void Awake() {
        _player = transform.parent.GetComponent<Player>();
    }
    public void TriggerOnMovementStateAnimationEnterEvent() {
        if (IsAnimationTransition()) {
            return;
        }
        _player.OnMovementStateAnimationEnterEvent();
    }public void TriggerOnMovementStateAnimationExitEvent() {
        if (IsAnimationTransition()) {
            return;
        }
        _player.OnMovementStateAnimationExitEvent();
    }public void TriggerOnMovementStateAnimationTransitionEvent() {
        if (IsAnimationTransition()) {
            return;
        }
        _player.OnMovementStateAnimationTransitionEvent();
    }
    public void OnThrowAnimationActuallyThrow()
    {
        if (IsAnimationTransition())
        {
            return;
        }
        _player.CoguCast.CastCogu();
    }
    public void OnThrowAnimationEnds()
    {
        if (IsAnimationTransition())
        {
            return;
        }
        _player.PlayerAnimator.SetBool("IsThrowing", false);
    }
    private bool IsAnimationTransition(int layerIndex = 0) {
        return _player.PlayerAnimator.IsInTransition(layerIndex);
    }
}
