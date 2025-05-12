using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TriggerEvent : ResetableBase
{
    [SerializeField] private bool _triggJustOnce;
    [SerializeField, TagField] private string _tagTrigger;

    [Space(8)]

    [SerializeField] private UnityEvent _triggerEvent;
    [SerializeField] private UnityEvent _resetEvent;

    private Collider _collider;
    private bool _once = true;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
    }

    public override void ResetObject()
    {
        if (NeedReset)
        {
            _resetEvent?.Invoke();
            _once = true;

            NeedReset = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((_triggJustOnce) ? _once : true)
        {
            if (other.CompareTag(_tagTrigger))
            {
                _triggerEvent?.Invoke();
                _once = false;

                NeedReset = true;
            }
        }       
    }
}
