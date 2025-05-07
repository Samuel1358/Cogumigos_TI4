using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TriggerEvent : MonoBehaviour
{
    [SerializeField] private bool _triggJustOnce;
    [SerializeField, TagField] private string _tagTrigger;

    [Space(8)]

    [SerializeField] private UnityEvent _event;

    private Collider _collider;
    private bool _once = true;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((_triggJustOnce) ? _once : true)
        {
            if (other.CompareTag(_tagTrigger))
            {
                _event?.Invoke();
                _once = false;
            }
        }       
    }
}
