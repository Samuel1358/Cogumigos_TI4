using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class Portal : MonoBehaviour {

    public UnityEvent EnterPortal;
    public UnityEvent ExitPortal;

    [SerializeField] private Portal _linkedPortal;
    [SerializeField] private float _timeToActivate;
    [SerializeField] private float _startTravel;
    [SerializeField] private VisualEffect _effect;

    private Transform _travelObject;
    private bool _canEnter = true;

    private void Start()
    {
        _effect.SetFloat("TeleportCD", _timeToActivate);
    }

    public Portal LinkedPortal {
        get { return _linkedPortal; }
        set { if(value is Portal)_linkedPortal = value; }
    }

    private void OnTriggerEnter(Collider collider) {
        if (!_canEnter)
            return;       

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
        yield return new WaitForSeconds(_timeToActivate + _startTravel);
        AudioManager.Instance.PlaySFX("TPIn");
        if (_travelObject != null) {
            Teleport();
        }
    }

    public virtual void Teleport() {
        _travelObject.position = _linkedPortal.transform.position;
        _travelObject.rotation = _linkedPortal.transform.rotation;
        _linkedPortal.JustArrived();

        AudioManager.Instance.PlaySFX("TPOut");
        _effect.gameObject.SetActive(false);
    }

    public void JustArrived()
    {
        _canEnter = false;
        TweenHandler.Timer(_startTravel, () => _canEnter = true);
    }
}