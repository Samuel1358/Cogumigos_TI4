using UnityEngine;
using UnityEngine.Events;

public class EndLevel : MonoBehaviour
{   
    [SerializeField] private UnityEvent _onJustBeforeEnd;
    [SerializeField] private UnityEvent _onEnd;

    private void Start()
    {
        if (TryGetComponent(out Collider collider))
            collider.isTrigger = true;
    }

    public void CallEnd()
    {
        _onJustBeforeEnd.Invoke();
        _onEnd.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        CallEnd();
    }
}
