using UnityEngine;

[RequireComponent(typeof(PlayerInput), typeof(PlayerInventory))]
[RequireComponent(typeof(CoguCastter), typeof(Rigidbody))]
public class Player : MonoBehaviour {
    [field: SerializeField] public PlayerSO Data { get; private set; }
    [field: SerializeField] public ResizableCapsuleCollider ColliderUtility { get; private set; }
    [field: SerializeField] public PlayerLayerData LayerData { get; private set; }
    [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }
    [field: SerializeField] public bool ShouldWalk { get; private set; }

    public PlayerInput Input { get; private set; }
    public PlayerInventory Inventory { get; private set; }
    public CoguCastter CoguCast { get; private set; }
    public Animator PlayerAnimator { get; private set; }
    public Rigidbody PlayerRigidbody { get; private set; }
    public bool ShouldGlide { get; private set; }
    private PlayerMovementStateMachine _movementStateMachine;

    #region TesteRespawn
    private int CoguCount = 0;
    private int _lastCount = 0;
    #endregion

    // TEMP
    public PlayerMovementStateMachine GetStateMachine() { return _movementStateMachine; }


    public Transform MainCameraTransform { get; private set; }
    private void Awake() {
        Input = GetComponent<PlayerInput>();
        Inventory = GetComponent<PlayerInventory>();
        CoguCast = GetComponent<CoguCastter>();
        PlayerRigidbody = GetComponent<Rigidbody>();
        PlayerAnimator = GetComponentInChildren<Animator>();
        _movementStateMachine = new PlayerMovementStateMachine(this);
    }
    private void Start() {
        AnimationData.Initialize();
        ColliderUtility.Initialize(gameObject);
        Inventory.Initialize();
        CoguCast.Initialize();
        ColliderUtility.CalculateCapsuleColliderDimensions();
        _movementStateMachine.ChangeState(_movementStateMachine.IdlingState);
        MainCameraTransform = Camera.main.transform;
        RespawnController.OnPlayerRespawn += ResetPlayer;
    }
    private void OnValidate() {
        ColliderUtility.Initialize(gameObject);
        ColliderUtility.CalculateCapsuleColliderDimensions();
    }
    private void Update() {
        _movementStateMachine.HandleInput();
        _movementStateMachine.Update();
    }
    private void FixedUpdate() {
        _movementStateMachine.PhysicsUpdate();
    }

    public void OnMovementStateAnimationEnterEvent() {
        _movementStateMachine.OnAnimationEnterEvent();
    }public void OnMovementStateAnimationExitEvent() {
        _movementStateMachine.OnAnimationExitEvent();
    }public void OnMovementStateAnimationTransitionEvent() {
        _movementStateMachine.OnAnimationTransitionEvent();
    }

    public void ResetPlayer() {
        CoguCount = _lastCount;
        transform.position = RespawnController.Instance.PlayerActiveCheckPoint.transform.position;
        transform.rotation = RespawnController.Instance.PlayerActiveCheckPoint.transform.rotation;
    }

    public void SetCheckpoint(int coguToAdd) {
        CoguCount += coguToAdd;
        _lastCount = CoguCount;
    }

    public void SetGlide(bool state) {
        ShouldGlide = state;
    }

    public void ChangeToTrampolineJumpState(float bounceForce) {
        _movementStateMachine.TrampolineJumpState.SetTrampolineForce(bounceForce);
        _movementStateMachine.ChangeState(_movementStateMachine.TrampolineJumpState);
    }
}
