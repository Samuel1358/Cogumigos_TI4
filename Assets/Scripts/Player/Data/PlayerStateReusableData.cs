using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerStateReusableData {
    public Vector2 MovementInput { get; set; }
    public float MovementSpeedModifier { get; set; } = 1f;
    public float MovementOnSlopeSpeedModifier { get; set; } = 1f;
    public float Gravity { get; private set; }
    public float CoyoteTimeCount { get; private set; }
    public float JumpBufferCount { get; private set; }
    // TORNAR PRIVATE SET DEPOIS
    public bool CanDoubleJump { get; set; }

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
    public PlayerRotationData RotationData { get; set; }

    public void SetGravity(float value) {
        Gravity = value;
    }
    public void SetCoyoteTime(float value) {
        CoyoteTimeCount = value;
    }
    public void SetJumpBuffer(float value) {
        JumpBufferCount = value;
    }
    public void EnableDoubleJump() {
        CanDoubleJump = true;
    }
    public void DisableDoubleJump() {
        CanDoubleJump = false;
    }
}
