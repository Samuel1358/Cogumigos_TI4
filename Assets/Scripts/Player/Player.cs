using UnityEngine;
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour {
    [field: SerializeField] public PlayerSO Data { get; private set; }
    [field: SerializeField] public PlayerCapsuleColliderUtility ColliderUtility { get; private set; }
    [field: SerializeField] public PlayerLayerData LayerData { get; private set; }
    [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }

    public PlayerInput Input { get; private set; }
    public Animator PlayerAnimator { get; private set; }
    public Rigidbody PlayerRigidbody { get; private set; }
    private PlayerMovementStateMachine _movementStateMachine;

    public Transform MainCameraTransform { get; private set; }
    private void Awake() {
        Input = GetComponent<PlayerInput>();
        PlayerRigidbody = GetComponent<Rigidbody>();
        PlayerAnimator = GetComponentInChildren<Animator>();
        _movementStateMachine = new PlayerMovementStateMachine(this);
    }
    private void Start() {
        AnimationData.Initialize();
        ColliderUtility.Initialize(gameObject);
        ColliderUtility.CalculateCapsuleColliderDimensions();
        _movementStateMachine.ChangeState(_movementStateMachine.IdlingState);
        MainCameraTransform = Camera.main.transform;
    }
    private void OnTriggerEnter(Collider collider) {
        _movementStateMachine.OntriggerEnter(collider);
    }
    private void OnTriggerExit(Collider collider) {
        _movementStateMachine.OntriggerExit(collider);
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
}
