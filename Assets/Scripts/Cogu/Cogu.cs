using System;
using UnityEngine;
using UnityEngine.AI;

public enum CoguType
{
    None,
    Trampoline,
    Plataform,
}

[RequireComponent(typeof(NavMeshAgent))]
public class Cogu : MonoBehaviour
{
    //[Header("Attributes")]
    protected CoguType _type = CoguType.None;
    protected Vector3 target;
    protected Transform targetFollow;
    // protected InteractiveObject interactiveObject;

    [Header("Settings")]
    [SerializeField] protected CoguSO data;

    protected NavMeshAgent _agent;
    public CoguStateMachine stateMachine { get; private set; }

    private void Awake()
    {
        stateMachine = new CoguStateMachine(this);
        stateMachine.ChangeState(stateMachine.idleState);
    }

    protected virtual void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    // Get & Set
    public CoguType GetCoguType()
    {
        return _type;
    }

    // Metodos Publicos
    #region // State Enters

    public void Stop()
    {
        target = default;
        targetFollow = null;

        target = default;
        targetFollow = null;
    }

    public void ArmyAttract(Transform _targetFollow) 
    {
        target = default;
        this.targetFollow = _targetFollow;

        _agent.enabled = true;
        _agent.speed = data.attractSpd;
    }

    public void JoinArmy(Transform _targetFollow)
    {
        target = default;
        this.targetFollow = _targetFollow;

        _agent.enabled = true;
        _agent.stoppingDistance = 1.5f;
        _agent.speed = data.followSpd;
    }

    public void Throw(Transform targetThrow)
    {
        target = targetThrow.position;
        targetFollow = null;

        _agent.enabled = true;
        _agent.stoppingDistance = 0f;
        _agent.speed = data.throwSpd;
    }

    #endregion

    #region // State Updates

    public void Chillin()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, data.interactRadius, data.interactIncludeLayers, QueryTriggerInteraction.Collide);
        foreach (Collider obj in colliders)
        {
            if (obj.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                stateMachine.ChangeState(StartInteracting(interactable));
                return;
            }
        }
    }

    public void Follow()
    {
        if (target != null)
        {
            _agent.SetDestination(Avoidence(targetFollow.position));
        }
    }

    public void Move()
    {
        if (target != null)
            _agent.SetDestination(target);
        else stateMachine.ChangeState(stateMachine.idleState);
    }

    public bool ArrivedDestination()
    {
        if (Vector3.Distance(transform.position, _agent.destination) < 0.2f)
            return true;
        else
            return false;
    }

    #endregion

    public void EndInteracting(Action act)
    {
        stateMachine.interactingState.EndInteracting(act);
    }

    // Private Methods
    private Vector3 Avoidence(Vector3 originalMove)
    {
        Collider[] nearby = Physics.OverlapSphere(transform.position, data.neighborPercieveRadius, data.neighborIncludeLayers);

        if (nearby.Length == 0)
            return originalMove;

        Vector3 avoidenceMove = Vector3.zero;
        int nAvoid = 0;

        foreach (Collider neighbor in nearby)
        {
            if (Vector3.SqrMagnitude(neighbor.transform.position - transform.position) < data.avoidenceDistance)
            {
                nAvoid++;
                avoidenceMove += transform.position - neighbor.transform.position;
            }
        }
        if (nAvoid != 0)
            avoidenceMove /= nAvoid;

        return originalMove + avoidenceMove;
    }

    private CoguState StartInteracting(IInteractable interactable)
    {
        return stateMachine.interactingState.StartInteracting(interactable.Interact(this));
    }
}
