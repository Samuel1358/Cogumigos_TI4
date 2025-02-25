using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Victor {
    public class PlayerStateReusableData {
        public Vector2 MovementInput { get; set; }
        public float MovementSpeedModifier { get; set; } = 1f;
        public bool ShouldWalk { get; set; }
        private Vector3 _currentTargetRotation;
        private Vector3 _timeToReachtargetRotation;
        private Vector3 _dampedTargetRotationCurrentVelocity;
        private Vector3 _dampedTargetRotationPassedTime;

        public ref Vector3 CurrentTargetRotation {
            get {
                return ref _currentTargetRotation;
            }
        }
        public ref Vector3 TimeToReachTargetRoation {
            get {
                return ref _timeToReachtargetRotation;
            }
        }
        public ref Vector3 DampedTargetRotationCurrentVelocity {
            get {
                return ref _dampedTargetRotationCurrentVelocity;
            }
        }
        public ref Vector3 DampedTargetRotationPassedTime {
            get {
                return ref _dampedTargetRotationPassedTime;
            }
        }
    }
}
