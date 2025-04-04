using System;
using UnityEngine;
[Serializable]
public class PlayerFallData {
    [field: SerializeField][field: Range(1f, 50f)] public float FallSpeedLimit { get; private set; } = 15f;
    [field: SerializeField][field: Range(1f, 100f)] public float MinimumDistanceToBeConsideredHardFall { get; private set; } = 3f;
}
