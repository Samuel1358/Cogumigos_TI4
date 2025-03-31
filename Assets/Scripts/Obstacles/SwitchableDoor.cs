using UnityEngine;

//[RequireComponent(typeof(Animation))]
public class SwitchableDoor : MonoBehaviour, ISwitchable
{
    [SerializeField] private Animation anim;
    [SerializeField] private AnimationClip openAnimation;
    [SerializeField] private AnimationClip closeAnimation;


    private void Start()
    {
        openAnimation.legacy = true;
        closeAnimation.legacy = true;
        //anim = GetComponent<Animation>();
    }

    // Public Interface Methods
    public void Activate()
    {
        OpenAnimation();
    }

    public void Disable()
    {
        CloseAnimation();
    }

    // Private Methods
    private void OpenAnimation()
    {
        anim.clip = openAnimation;
        anim?.Play();
    }

    private void CloseAnimation()
    {
        anim.clip = closeAnimation;
        anim?.Play();
    }
}
