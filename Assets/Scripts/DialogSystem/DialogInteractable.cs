using UnityEngine;

namespace DialogSystem
{
    public class DialogInteractable : MonoBehaviour
    {
        [SerializeField] private DialogData dialogData;
        [SerializeField] private bool autoPlay = false;
        [SerializeField] private KeyCode interactionKey = KeyCode.E;
        
        private DialogController dialogController;
        private bool playerInRange;
        
        private void Start()
        {
            dialogController = FindFirstObjectByType<DialogController>();
            
            if (autoPlay)
            {
                StartDialog();
            }
        }
        
        private void Update()
        {
            if (dialogController == null)
            {
                return;
            }

            if (playerInRange && Input.GetKeyDown(interactionKey) && !dialogController.IsDialogActive)
            {
                StartDialog();
            }
            
            if (dialogController.IsDialogActive && Input.GetKeyDown(interactionKey))
            {
                dialogController.AdvanceDialog();
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                playerInRange = true;
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                playerInRange = false;
            }
        }
        
        public void StartDialog()
        {
            if (dialogData == null || dialogController == null)
            {
                return;
            }
            
            dialogController.StartDialog(dialogData);
        }
    }
} 