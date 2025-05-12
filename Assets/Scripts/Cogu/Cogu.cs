using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Cogu : MonoBehaviour
{
    // Fields
    [SerializeField] private CoguData _data;

    private CoguStateMachine _stateMachine;
    private NavMeshAgent _agent;
    private CoguInteractable _interactableObj;
    private CoguCastter _castter;

    // Properties
    public CoguData Data { get { return _data; } }
    public CoguStateMachine StateMachine { get { return _stateMachine; } }
    public NavMeshAgent Agent { get { return _agent; } }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    // Public Methods
    public void Initialize(Vector3 interactSpot, CoguInteractable interactable, CoguCastter castter)
    {
        _stateMachine = new CoguStateMachine(this);
        _stateMachine.ChangeState(_stateMachine.CastState.Setup(interactSpot));
        this._interactableObj = interactable;
        this._castter = castter;

        CoguManager.instance.AssingCogu(this);
    }

    public bool ArrivedDestination()
    {
        if (Vector3.Distance(transform.position, _agent.destination) <= _data.minDistance)
            return true;
        else
            return false;
    }

    public void ResetAnableCast()
    {
        _castter.IsAbleCast = true;
    }

    public Action StartInteracting()
    {
        _interactableObj.DisableInteract();
        return _interactableObj.Interact(this);
    }

    public void EndInteracting(Action act)
    {
        _stateMachine.InteractState.EndInteracting(act);
    }

    public void SelfDestruction()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        CoguManager.instance.RemoveCogu(this);
    }
}
