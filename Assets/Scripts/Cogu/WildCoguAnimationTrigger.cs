using UnityEngine;

public class WildCoguAnimationTrigger : MonoBehaviour
{
    [SerializeField] private WildCogu _cogu;

    public void TriggerOnDisapearingAnimationEnd()
    {
        if (_cogu != null)
            _cogu.Collect();
    }

    public void TirggerOnIdleVariantEnd()
    {
        if (_cogu != null)
            _cogu.Animator.SetInteger("RandIdle", 0);

    }
}
