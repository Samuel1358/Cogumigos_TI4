using UnityEngine;

[RequireComponent(typeof(Animation))]
public class SwitchableDoor : MonoBehaviour, ISwitchable
{
    [SerializeField] AnimationClip openAnimation;
    [SerializeField] AnimationClip closeAnimation;

    private Animation anim;

    private void Start()
    {
        anim = GetComponent<Animation>();
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
