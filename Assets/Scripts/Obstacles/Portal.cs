using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Portal : MonoBehaviour {

    public UnityEvent EnterPortal;
    public UnityEvent ExitPortal;

    [SerializeField] private Portal _linkedPortal;
    [SerializeField] private float _timeToActivate;

    private Transform _travelObject;

    public Portal LinkedPortal {
        get { return _linkedPortal; }
        set { if(value is Portal)_linkedPortal = value; }
    }

    private void OnTriggerEnter(Collider collider) {
        if (collider.transform.parent.TryGetComponent<Player>(out Player player)) {
            _travelObject = player.transform;
            EnterPortal.Invoke();

            StartCoroutine(StartTravel());
            
        }
    }

    private void OnTriggerExit(Collider collider) {
        if (collider.transform.parent.TryGetComponent<Player>(out Player player)) {
            ExitPortal.Invoke();
            _travelObject = null;
            StopAllCoroutines();
        }
    }

    void OnValidate() {
        if (_linkedPortal != null) {
            _linkedPortal.LinkedPortal = this;
        }
    }

    private IEnumerator StartTravel() {      
        yield return new WaitForSeconds(_timeToActivate);
        GameIniciator.Instance.AudioManagerInstance.PlaySFX("TPIn");
        if (_travelObject != null) {
            Teleport();
        }
    }

    public virtual void Teleport() {
        _travelObject.position = _linkedPortal.transform.position;
        _travelObject.rotation = _linkedPortal.transform.rotation;
        GameIniciator.Instance.AudioManagerInstance.PlaySFX("TPOut");
    }

}