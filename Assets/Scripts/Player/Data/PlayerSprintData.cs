using System;
using UnityEngine;
[Serializable]
public class PlayerSprintData {
    [field: SerializeField][field: Range(1f, 3f)] public float SpeedModifier { get; private set; } = 1.7f;
}
