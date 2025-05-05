using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class TEST_Cogu : MonoBehaviour
{
    // Fields
    [SerializeField] private TEST_CoguData _data;

    private TEST_CoguStateMachine _stateMachine;
    private NavMeshAgent _agent;
    private CoguInteractable _interactableObj;
    private TEST_CoguCastter _castter;

    // Properties
    public TEST_CoguData Data { get { return _data; } }
    public TEST_CoguStateMachine StateMachine { get { return _stateMachine; } }
    public NavMeshAgent Agent { get { return _agent; } }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    // Public Methods
    public void Initialize(Vector3 interactSpot, CoguInteractable interactable, TEST_CoguCastter castter)
    {
        _stateMachine = new TEST_CoguStateMachine(this);
        _stateMachine.ChangeState(_stateMachine.CastState.Setup(interactSpot));
        this._interactableObj = interactable;
        this._castter = castter;

        TEST_CoguManager.instance.AssingCogu(this);
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
        return _interactableObj.TEST_Interact(this);
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
        TEST_CoguManager.instance.RemoveCogu(this);
    }
}
