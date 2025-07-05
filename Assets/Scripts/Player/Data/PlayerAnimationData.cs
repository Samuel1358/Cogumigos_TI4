using System;
using UnityEngine;
[Serializable]
public class PlayerAnimationData {
    [Header("State Group Parameter Names")]
    [SerializeField] private string groundedParameterName = "Grounded";
    [SerializeField] private string movingParameterName = "Moving";
    [SerializeField] private string landingParameterName = "Landing";
    [SerializeField] private string airborneParameterName = "Airborne";

    [Header("Grounded Parameter Names")]
    [SerializeField] private string idleParameterName = "isIdling";
    [SerializeField] private string dashParameterName = "isDashing";
    [SerializeField] private string walkParameterName = "isWalking";
    [SerializeField] private string runParameterName = "isRunning";
    [SerializeField] private string sprintParameterName = "isSprinting";
    [SerializeField] private string rollParameterName = "isRolling";
    [SerializeField] private string hardLandParameterName = "isHardLanding";

    [Header("Air Parameter Names")]
    [SerializeField] private string fallParameterName = "isFalling";
    [SerializeField] private string glideParameterName = "IsGliding";
    [SerializeField] private string isDoubleJump = "IsDoubleJump";

    [Header("Throw Parameter Names")]
    [SerializeField] private string isThrowing = "IsThrowing";



    public int GroundedParameterHash { get; private set; }
    public int MovingParameterHash { get; private set; }
    public int LandingParameterHash { get; private set; }
    public int AirborneParameterHash { get; private set; }

    public int IdleParameterHash { get; private set; }
    public int DashParameterHash { get; private set; }
    public int WalkParameterHash { get; private set; }
    public int RunParameterHash { get; private set; }
    public int SprintParameterHash { get; private set; }
    public int RollParameterHash { get; private set; }
    public int HardLandParameterHash { get; private set; }

    public int FallParameterHash { get; private set; }
    public int GlideParameterHash { get; private set; }
    public int IsDoubleJump { get; private set; }

    public int IsThrowing { get; private set; }

    public void Initialize() {
        GroundedParameterHash = Animator.StringToHash(groundedParameterName);
        MovingParameterHash = Animator.StringToHash(movingParameterName);
        LandingParameterHash = Animator.StringToHash(landingParameterName);
        AirborneParameterHash = Animator.StringToHash(airborneParameterName);

        IdleParameterHash = Animator.StringToHash(idleParameterName);
        DashParameterHash = Animator.StringToHash(dashParameterName);
        WalkParameterHash = Animator.StringToHash(walkParameterName);
        RunParameterHash = Animator.StringToHash(runParameterName);
        SprintParameterHash = Animator.StringToHash(sprintParameterName);
        RollParameterHash = Animator.StringToHash(rollParameterName);
        HardLandParameterHash = Animator.StringToHash(hardLandParameterName);

        FallParameterHash = Animator.StringToHash(fallParameterName);
        GlideParameterHash = Animator.StringToHash(glideParameterName);
        IsDoubleJump = Animator.StringToHash(isDoubleJump);

        IsThrowing = Animator.StringToHash(isThrowing);
    }
}
