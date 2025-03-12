using System;
using UnityEngine;
[Serializable]
public class PlayerGlideData {
    [field: SerializeField][field: Range(0f, 3f)] public float SpeedModifier { get; private set; }
    [field: SerializeField][field: Range(0f, 5f)] public float FallSpeedLimit { get; private set; }
}
