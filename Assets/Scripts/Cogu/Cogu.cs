using System;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(NavMeshAgent))]
public class Cogu : MonoBehaviour
{
    // Fields
    [SerializeField] private CoguData _data;
    [SerializeField] private Animator _animator;
    [SerializeField] private bool _lookAt;
    [SerializeField] private LookAtTarget _lookAtTarget;

    private CoguStateMachine _stateMachine;
    //private NavMeshAgent _agent;
    private CoguInteractable _interactableObj;
    private CoguCastter _castter;
    private Vector3 _castSpot;
    private Vector3 _interactSpot;

    // Properties
    public CoguData Data { get { return _data; } }
    public Animator Animator { get { return _animator; } }
    public LookAtTarget LookAtTarget { get { return _lookAtTarget; } }
    public CoguStateMachine StateMachine { get { return _stateMachine; } }
    //public NavMeshAgent Agent { get { return _agent; } }
    public Vector3 CastSpot { get { return _castSpot; } }
    public Vector3 InteractSpot {  get { return _interactSpot; } }

    /*private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.enabled = false;
    }*/

    // Public Methods
    public void Initialize(CoguInteractable interactable, CoguCastter castter)
    {
        _stateMachine = new CoguStateMachine(this);
        _stateMachine.ChangeState(_stateMachine.CastState);
        this._interactableObj = interactable;
        this._castter = castter;

        _castSpot = transform.position;
        float t = interactable.InteractDistance / (transform.position - interactable.transform.position).magnitude;
        _interactSpot = Vector3.Lerp(interactable.transform.position, transform.position, t);

        if (_lookAt)
            _lookAtTarget.SetTarget(_interactableObj.transform);
        
        CoguManager.instance.AssingCogu(this);
    }

    /*public bool ArrivedDestination()
    {
        if (Vector3.Distance(transform.position, _agent.destination) <= _interactableObj.InteractDistance)
            return true;
        else
            return false;
    }*/

    public void ResetAnableCast()
    {
        _castter.IsAbleCast = true;
    }

    public void StartInteracting()
    {
        _interactableObj.DisableInteract();
        _interactableObj.Interact(this);
    }

    public void EndInteracting(Action act)
    {
        _stateMachine.InteractState.EndInteracting(act);
    }

    public void SelfDestruction()
    {
        Destroy(gameObject);
    }

    // Animation
    public void AnimToThrow()
    {
        _stateMachine.ChangeState(_stateMachine.ThrowState);
        Animator.SetBool("Throw", true);
    }

    public void AnimToMove()
    {
        //_stateMachine.ChangeState(_stateMachine.MoveState);
        Animator.SetBool("Move", true);
    }

    public void InteractionOnAnim()
    {        
        _stateMachine.InteractState.Interact();
    }

    private void OnDestroy()
    {
        CoguManager.instance.RemoveCogu(this);
    }
}
