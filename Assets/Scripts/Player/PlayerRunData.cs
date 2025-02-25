using System;
using UnityEngine;

namespace Victor
{
    [Serializable]
    public class PlayerRunData
    {
        [field: SerializeField][field: Range(0f, 2f)] public float SpeedModifier { get; private set; } = 1f;
    }
}
