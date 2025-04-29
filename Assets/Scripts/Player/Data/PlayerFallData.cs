using System;
using UnityEngine;
[Serializable]
public class PlayerFallData {
    [field: SerializeField][field: Range(1f, 50f)] public float FallMultiplier { get; private set; } = 4.0f;
    [field: SerializeField][field: Range(1f, 100f)] public float MinimumDistanceToBeConsideredHardFall { get; private set; } = 3f;
}
