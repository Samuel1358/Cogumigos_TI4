using UnityEngine;

public class RemovableWaypoint : MonoBehaviour
{
    [SerializeField, Min(0.001f)] private float _speedModifier = 1f;
    [SerializeField] private float _changeDistanceModifier;

    public float SpeedModifier { get { return _speedModifier; } }
    public float ChangeDistanceModifier { get { return _changeDistanceModifier; } }

    private float ChangeDistance()
    {
        if (_changeDistanceModifier == 0f)
            return 0.01f;
        else
            return _changeDistanceModifier;
    }
}
