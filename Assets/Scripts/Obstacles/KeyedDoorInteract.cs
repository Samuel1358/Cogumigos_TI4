using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class KeyedDoorInteract : MonoBehaviour
{
    [Header("Esternal Accesses")]
    [SerializeField] private Switchable _switchable;
    [SerializeField] private GameObject _visualInfo;

    [Header("Settings")]
    [SerializeField] private KeyTypes _keyAccepted;
    [SerializeField, TagField] private string _tagTrigger;

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

    // MonoBehaviour Methods
    private void OnTriggerEnter(Collider other)
    {
        if (!_isInteracted)
        {
            if (other.CompareTag(_tagTrigger))
            {
                Debug.Log("Keyed Enter");
                TEMP_InputManager.instance.onInteractInput += Interact;

                // visual
                if (_visualInfo != null)
                    _visualInfo.SetActive(true);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!_isInteracted && _isInteracting)
        {
            if (other.CompareTag(_tagTrigger))
            {
                if (other.TryGetComponent(out Player player))
                {
                    if (player.Inventory.TryUseKey(_keyAccepted))
                    {
                        _switchable.Activate();
                        _isInteracted = true;
                    }
                }
            }
        }       
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_isInteracted)
        {
            if (other.CompareTag(_tagTrigger))
            {
                Debug.Log("Keyed Exit");
                TEMP_InputManager.instance.onInteractInput -= Interact;

                // visual
                if (_visualInfo != null)
                    _visualInfo.SetActive(false);
            }
        }
    }
}
