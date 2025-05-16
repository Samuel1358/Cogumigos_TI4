using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionRange = 2f;
    public KeyCode interactionKey = KeyCode.E;
    public LayerMask interactableLayer;

    private InteractableObject currentInteractable;

    void Update()
    {
        // Procura por objetos interativos próximos
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRange, interactableLayer);
        
        // Encontra o objeto mais próximo
        float closestDistance = float.MaxValue;
        InteractableObject closestObject = null;

        foreach (Collider collider in colliders)
        {
            InteractableObject interactable = collider.GetComponent<InteractableObject>();
            if (interactable != null)
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = interactable;
                }
            }
        }

        // Atualiza o objeto atual
        currentInteractable = closestObject;

        // Interage com o objeto quando a tecla é pressionada
        if (Input.GetKeyDown(interactionKey) && currentInteractable != null)
        {
            currentInteractable.Toggle();
        }
    }

    void OnDrawGizmosSelected()
    {
        // Desenha o range de interação no editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
} 