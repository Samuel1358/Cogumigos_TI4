using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Cogu : MonoBehaviour
{
    [Header("Attributes")]
    protected FriendshroomType _type = FriendshroomType.Basic;
    //[SerializeField] protected FriendshroomStates state = FriendshroomStates.Idle;
    [SerializeField] protected Vector3 target;
    [SerializeField] protected Transform targetFollow;
    [SerializeField] protected InteractiveObject interactiveObject;

    [Header("Settings")]
    [SerializeField] protected float attractSpd;
    [SerializeField] protected float followSpd;
    [SerializeField] protected float throwSpd;
    [SerializeField] protected float interactRadius;
    [SerializeField] protected LayerMask interactIncludeLayers;
    [SerializeField] protected float neighborPercieveRadius;
    [SerializeField] protected LayerMask neighborIncludeLayers;
    [SerializeField] protected float avoidenceDistance;

    protected NavMeshAgent _agent;
    public CoguStateMachine stateMachine { get; private set; }

    private void Awake()
    {
        stateMachine = new CoguStateMachine(this);
    }

    protected virtual void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    // Get & Set
    public FriendshroomType GetFriendshroomType()
    {
        return _type;
    }

    /*public FriendshroomStates GetState()
    {
        return state;
    }*/

    #region

    public Vector3 GetTarget()
    {
        return target;
    }
    public void SetTarget(Vector3 target)
    {
        this.target = target;
    }

    public Transform GetTargetFollow()
    {
        return targetFollow;
    }
    public void SetTargetFollow(Transform targetFollow)
    {
        this.targetFollow = targetFollow;
    }

    public float GetAttractSpd()
    {
        return attractSpd;
    }

    public float GetFollowSpd()
    {
        return followSpd;
    }

    public float GetThrowSpd()
    {
        return throwSpd;
    }

    public NavMeshAgent GetAgent()
    {
        return _agent;
    }

    #endregion

    /*protected virtual void SetState(FriendshroomStates _state)
    {
        this.state = _state;

        switch (_state)
        {
            case FriendshroomStates.Idle:
                // _agent.enabled = false;
                break;
            case FriendshroomStates.Attract:
                _agent.enabled = true;
                _agent.speed = attractSpd;

                break;
            case FriendshroomStates.Follow:
                _agent.enabled = true;
                _agent.stoppingDistance = 1.5f;
                _agent.speed = followSpd;
                break;
            case FriendshroomStates.Throw:
                _agent.enabled = true;
                _agent.stoppingDistance = 0f;
                _agent.speed = throwSpd;
                break;
            case FriendshroomStates.Carry:
                _agent.enabled = false;
                break;
            case FriendshroomStates.Convey:
                _agent.enabled = false;
                break;
        }
    }*/

    // Metodos Publicos
    #region // Updates

    public void Chillin()
    {
        Collider[] around = Physics.OverlapSphere(transform.position, interactRadius, interactIncludeLayers);

        if (around.Length == 0)
            return;

        foreach (var item in around)
        {
            if (InheritChillinItem(item))
                break;

            var obj = item.GetComponent<InteractiveObject>();
            if (obj != null)
            {
                interactiveObject = obj;

                obj.AssingFriendshroom();
                obj.PositionFriendshroom(transform);

                /*switch (obj.GetInteractiveType())
                {
                    case InteractiveObjectType.Carry:
                        Carry();
                        break;
                    case InteractiveObjectType.Convey:
                        Convey();
                        break;
                }*/

                break;
            }
        }
    }

    public void Follow() //
    {
        if (target != null)
        {
            _agent.SetDestination(Avoidence(targetFollow.position));
        }
    }

    public void Move() //
    {
        if (target != null)
            _agent.SetDestination(target);
    }

    public bool ArrivedDestination()
    {
        if (Vector3.Distance(transform.position, _agent.destination) < 0.2f)
            return true;
        else
            return false;
    }

    #endregion

    #region // State Shifters

    public void Stop() //
    {
        target = default;
        targetFollow = null;
        stateMachine.ChangeState(stateMachine.idleState);
    }

    public void ArmieAttract(Transform _targetFollow) //
    {
        target = default;
        this.targetFollow = _targetFollow;
        stateMachine.ChangeState(stateMachine.attractState);
    }

    public void JoinArmie(Transform _targetFollow)
    {
        target = default;
        this.targetFollow = _targetFollow;
        stateMachine.ChangeState(stateMachine.followState);
    }

    public void Throw(Transform targetThrow) //
    {
        target = targetThrow.position;
        targetFollow = null;
        stateMachine.ChangeState(stateMachine.throwState);
    }

    #endregion

    public void StopCarry()
    {
        transform.SetParent(transform);

        interactiveObject.RealeaseFriendshroom();
        interactiveObject = null;
    }

    public Vector3 Avoidence(Vector3 originalMove)
    {
        Collider[] nearby = Physics.OverlapSphere(transform.position, neighborPercieveRadius, neighborIncludeLayers);

        if (nearby.Length == 0)
            return originalMove;

        Vector3 avoidenceMove = Vector3.zero;
        int nAvoid = 0;

        foreach (Collider neighbor in nearby)
        {
            if (Vector3.SqrMagnitude(neighbor.transform.position - transform.position) < avoidenceDistance)
            {
                nAvoid++;
                avoidenceMove += transform.position - neighbor.transform.position;
            }
        }
        if (nAvoid != 0)
            avoidenceMove /= nAvoid;

        return originalMove + avoidenceMove;
    }

    // Metodos Privados

    protected virtual bool InheritChillinItem(Collider item) { return false; }
}
