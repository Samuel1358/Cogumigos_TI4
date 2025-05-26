using System;
using UnityEngine;
[Serializable]
public class PlayerJumpData {
    [field: SerializeField][field: Range(0f, 1f)] public float MaxJumpTime { get; private set; } = 0f;
    [field: SerializeField][field: Range(0f, 10f)] public float MaxJumpHeight { get; private set; } = 0f;
    [field: SerializeField][field: Range(0f, 1f)] public float CoyoteTime { get; private set; } = 0.3f;
    [field: SerializeField][field: Range(0f, 1f)] public float JumpBuffer { get; private set; } = 0.2f;
}
