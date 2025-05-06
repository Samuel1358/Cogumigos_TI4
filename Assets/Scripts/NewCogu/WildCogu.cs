using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class WildCogu : MonoBehaviour
{
    // Fields
    [SerializeField] private CoguData _data;

    private WildCoguStateMachine _stateMachine;
    private NavMeshAgent _agent;

    // Properties
    public CoguData Data { get { return _data; } }
    public WildCoguStateMachine StateMachine {  get { return _stateMachine; } }
    public NavMeshAgent Agent { get { return _agent; } }

    private void Awake()
    {
        _stateMachine = new WildCoguStateMachine(this);
        _stateMachine.ChangeState(_stateMachine.IdleState);
    }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    // Public Methods
    public void Attract(CoguCastPoint assingPoint)
    {
        if (_stateMachine.GetCurrentState() == _stateMachine.IdleState)
        {
            _stateMachine.ChangeState(_stateMachine.AttracState.Setup(assingPoint));
        }
    }

    public bool ArrivedDestination()
    {
        if (Vector3.Distance(transform.position, _agent.destination) <= _stateMachine.WildCogu.Data.minDistance)
            return true;
        else
            return false;
    }

    private void OnDestroy()
    {
        CoguManager.instance.RemoveWildCogu(this);
    }
}
