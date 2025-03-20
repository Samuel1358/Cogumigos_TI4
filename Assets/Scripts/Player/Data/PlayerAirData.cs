using System;
using UnityEngine;
[Serializable]
public class PlayerAirData {
    [field: SerializeField] public PlayerJumpData JumpData { get; private set; }
    [field: SerializeField] public PlayerFallData FallData { get; private set; }
    [field: SerializeField] public PlayerGlideData GlideData { get; private set; }
}
