using UnityEngine;
using UnityEngine.VFX;

public class VFXAnimationTrigger : MonoBehaviour
{
    [SerializeField] private VisualEffect[] _effects;

    public void TriggerEffect(int id)
    {
        _effects[id].Play();
    }
}
