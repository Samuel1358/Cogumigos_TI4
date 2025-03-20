using UnityEngine;

public class SwitchableDoor : /*MonoBehaviour,*/ ISwitchable
{
    [SerializeField] Animation openAnimation;
    [SerializeField] Animation closeAnimation;

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
        openAnimation.Play();
    }

    private void CloseAnimation()
    {
        closeAnimation.Play();
    }
}
