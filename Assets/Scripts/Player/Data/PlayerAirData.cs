using System;
using UnityEngine;
[Serializable]
public class PlayerAirData {
    public float Gravity { get; private set; }
    [field: SerializeField] public PlayerJumpData JumpData { get; private set; }
    [field: SerializeField] public PlayerFallData FallData { get; private set; }
    [field: SerializeField] public PlayerGlideData GlideData { get; private set; }

    public void SetGravity(float value) {
        Gravity = Mathf.Clamp(value, 0f, 10f);
    }
}
