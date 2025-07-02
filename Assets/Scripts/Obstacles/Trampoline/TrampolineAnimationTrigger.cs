using UnityEngine;

public class TrampolineAnimationTrigger : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public void EnableBounce()
    {
        _animator.SetBool("IsBounce", true);
    }

    public void DisableBounce()
    {
        _animator.SetBool("IsBounce", false);
    }
}
