using System.Collections.Generic; 
using UnityEngine;

public class CoguInteractableSensor : MonoBehaviour
{
    [SerializeField] private CoguCastter _coguCastter;
    [SerializeField] private bool _soloWithCogu;
    private List<CoguInteractable> _interactables = new List<CoguInteractable>();

    private void Update()
    {
        if ((_soloWithCogu) ? _coguCastter.CoguCount <= 0 : false)
            return;

        Collider[] colliders = Physics.OverlapSphere(transform.position, _coguCastter.InteractRadius, _coguCastter.InteractableLayer);
        List<CoguInteractable> aux = new List<CoguInteractable>();

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out CoguInteractable interactable))
            {
                Verify(interactable, aux);
            }
        }

        if (_interactables.Count <= 0)
            return;

        foreach (CoguInteractable interactable in _interactables)
        {
            if (!aux.Contains(interactable))
            {
                interactable.SetActiveInteractableEffectVisual(false);
                _interactables.Remove(interactable);
                //Debug.Log("SENSOR - remove - " + interactable.gameObject);
            }
        }
    }

    private void Verify(CoguInteractable interactable, List<CoguInteractable> inList)
    {      
        if (_interactables.Contains(interactable))
        {
            inList.Add(interactable);
        }
        else
        {
            inList.Add(interactable);

            interactable.SetActiveInteractableEffectVisual(true);
            _interactables.Add(interactable);
            //Debug.Log("SENSOR - add - " + interactable.gameObject);
        }
    }
}
