using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    [Header("Interaction Settings")]
    public bool isActivated = false;
    public Light objectLight;

    [Header("Events")]
    public UnityEvent onActivate;
    public UnityEvent onDeactivate;

    private void Start()
    {
        if (objectLight == null)
        {
            objectLight = GetComponentInChildren<Light>();
        }
        
        if (objectLight != null)
        {
            objectLight.enabled = isActivated;
        }
    }

    public void Toggle()
    {
        if (isActivated)
        {
            Deactivate();
        }
        else
        {
            Activate();
        }
    }

    public virtual void Activate()
    {
        if (!isActivated)
        {
            isActivated = true;
            if (objectLight != null)
            {
                objectLight.enabled = true;
            }
            onActivate?.Invoke();
        }
    }

    public virtual void Deactivate()
    {
        if (isActivated)
        {
            isActivated = false;
            if (objectLight != null)
            {
                objectLight.enabled = false;
            }
            onDeactivate?.Invoke();
        }
    }
} 