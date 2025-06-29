using UnityEngine;

public class CoguAnimationTrigger : MonoBehaviour
{
    [SerializeField] private Cogu _cogu;

    public void TriggerOnCastEnd()
    {
        if (_cogu == null)
            return;

        _cogu.AnimToThrow();
    }

    public void TriggerOnThrowEnd()
    {
        if (_cogu == null)
            return;

        _cogu.AnimToMove();
    }

    public void TriggerOnInteract()
    {
        if (_cogu == null)
            return;

        _cogu.InteractionOnAnim();
    }
}
