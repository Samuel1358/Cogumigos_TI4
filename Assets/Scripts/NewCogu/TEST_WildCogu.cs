using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class TEST_WildCogu : MonoBehaviour
{
    // Fields
    [SerializeField] private TEST_CoguData _data;

    private TEST_WildCoguStateMachine _stateMachine;
    private NavMeshAgent _agent;

    // Properties
    public TEST_CoguData Data { get { return _data; } }
    public TEST_WildCoguStateMachine StateMachine {  get { return _stateMachine; } }
    public NavMeshAgent Agent { get { return _agent; } }

    private void Awake()
    {
        _stateMachine = new TEST_WildCoguStateMachine(this);
        _stateMachine.ChangeState(_stateMachine.IdleState);
    }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    // Public Methods
    public void Attract(TEST_CoguCastPoint assingPoint)
    {
        //Debug.Log("1");
        if (_stateMachine.GetCurrentState() == _stateMachine.IdleState)
        {
            _stateMachine.ChangeState(_stateMachine.AttracState.Setup(assingPoint));
        }
    }

    public void SelfDestruction()
    {
        TEST_CoguManager.instance.RemoveWildCogu(this);
        Destroy(gameObject);
    }
}
