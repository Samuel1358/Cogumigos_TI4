using UnityEngine;
public class WildCogu : ResetableBase
{
    [SerializeField] private GameObject _visual;

    private WildCoguStateMachine _stateMachine;
    private Collider _collider;
    private Vector3 _initialPosition;

    public WildCoguStateMachine StateMachine {  get { return _stateMachine; } }
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
    }
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
            _visual.SetActive(true);
            _collider.enabled = true;
            _stateMachine.Reset();

            NeedReset = false;
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        CoguCastter castter = other.GetComponentInParent<CoguCastter>();
        if (castter != null)
        {
            StateMachine.ChangeState(StateMachine.DisappearState.Setup(castter.CastPoint));
        }
    }

    private void OnDestroy()
    {
        CoguManager.instance.RemoveWildCogu(this);
    }
}
