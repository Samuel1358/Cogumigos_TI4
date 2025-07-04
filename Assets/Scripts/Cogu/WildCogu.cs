using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class WildCogu : ResetableBase
{
    // Fields
    //[SerializeField] private CoguData _data;
    [SerializeField] private GameObject _visual;
    [SerializeField] private Animator _animator;

    private WildCoguStateMachine _stateMachine;
    private Collider _collider;

    private Vector3 _initialPosition;

    // Properties
    //public CoguData Data { get { return _data; } }
    public WildCoguStateMachine StateMachine {  get { return _stateMachine; } }
    public Animator Animator { get { return _animator; } }

    private CoguCastPoint _castPoint;

    private void Awake()
    {
        _stateMachine = new WildCoguStateMachine(this);
        _stateMachine.ChangeState(_stateMachine.IdleState);

        _initialPosition = transform.position;
    }

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
        //_agent = GetComponent<NavMeshAgent>();       
    }

    // Public Methods
    /*public void Attract(CoguCastPoint assingPoint)
    {
        if (_stateMachine.GetCurrentState() == _stateMachine.IdleState)
        {
            _stateMachine.ChangeState(_stateMachine.AttracState.Setup(assingPoint));
        }
    }*/

    /*public bool ArrivedDestination()
    {
        if (Vector3.Distance(transform.position, _agent.destination) <= _stateMachine.WildCogu.Data.minDistance)
            return true;
        else
            return false;
    }*/

    public void Disable()
    {
        _visual.SetActive(false);
        _collider.enabled = false;

        NeedReset = true;
    }

    public override void ResetObject()
    {
        if (NeedReset)
        {
            transform.position = _initialPosition;
            //_agent.enabled = true;
            //_agent.SetDestination(transform.position);
            _visual.SetActive(true);
            _collider.enabled = true;
            _stateMachine.Reset();

            NeedReset = false;
        }        
    }

    public void Collect()
    {
        if (_castPoint == null)
            return;

        StateMachine.ChangeState(StateMachine.DisappearState.Setup(_castPoint));
    }

    private void OnTriggerEnter(Collider other)
    {
        CoguCastter castter = other.GetComponentInParent<CoguCastter>();
        if (castter != null)
        {
            StateMachine.ChangeState(StateMachine.AttracState);
            _castPoint = castter.CastPoint;
        }
    }

    private void OnDestroy()
    {
        GameIniciator.Instance.CoguManagerInstance.RemoveWildCogu(this);
    }
}
