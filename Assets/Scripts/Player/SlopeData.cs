using System;
using UnityEngine;
namespace Victor {
    [Serializable]
    public class SlopeData {
        [field: SerializeField]
        [field: Range(0f, 1f)]
        public float StepHeightPercentege { get; private set; } = 0.25f;
    }
}
