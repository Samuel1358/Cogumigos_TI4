using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class KeyedDoorInteract : ResetableBase
{
    [Header("Esternal Accesses")]
    [SerializeField] private Switchable _switchable;
    [SerializeField] private GameObject _visualInfo;

    [Header("Settings")]
    [SerializeField] private KeyTypes _keyAccepted;

    private bool _isInteracting = false;
    private bool _isInteracted = false;

    private void Start()
    {
        GetComponent<Collider>().isTrigger = true;
        if (_visualInfo != null)
            _visualInfo.SetActive(false);
    }

    // Private Methods
    private void Interact()
    {
        _isInteracting = true;
    }

    public override void ResetObject()
    {
        if (NeedReset)
        {
            _isInteracting = false;
            _isInteracted = false;

            NeedReset = false;
        }
    }

    // MonoBehaviour Methods
    private void OnTriggerEnter(Collider other)
    {
        if (!_isInteracted)
        {
            TEMP_InputManager.instance.onInteractInput += Interact;

            // visual
            if (_visualInfo != null)
                _visualInfo.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!_isInteracted && _isInteracting)
        {
            Player player = other.GetComponentInParent<Player>();
            if (player == null)
                return;

            if (player.Inventory.TryUseKey(_keyAccepted))
            {
                _switchable.Activate();
                _isInteracted = true;

                NeedReset = true;
            }
        }       
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_isInteracted)
        {
            TEMP_InputManager.instance.onInteractInput -= Interact;

            // visual
            if (_visualInfo != null)
                _visualInfo.SetActive(false);
        }
    }
}
