using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class TEST_Cogu : MonoBehaviour
{
    // Fields
    [SerializeField] private TEST_CoguData _data;

    private TEST_CoguStateMachine _stateMachine;
    private NavMeshAgent _agent;

    // Properties
    public TEST_CoguData Data { get { return _data; } }
    public TEST_CoguStateMachine StateMachine { get { return _stateMachine; } }
    public NavMeshAgent Agent { get { return _agent; } }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    // Public Methods
    public void Initialize()
    {
        _stateMachine = new TEST_CoguStateMachine(this);
        _stateMachine.ChangeState(_stateMachine.CastState);
    }
}
