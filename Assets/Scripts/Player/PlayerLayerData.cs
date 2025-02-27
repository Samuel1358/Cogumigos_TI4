using System;
using UnityEngine;

[Serializable]
public class PlayerLayerData{
    [field: SerializeField] public LayerMask GroundLayerMask { get; private set; }
}
