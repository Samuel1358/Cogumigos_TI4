using System;
using UnityEngine;

namespace DialogSystem
{
    public class DialogController : MonoBehaviour
    {
        public static DialogController instance;

        [SerializeField] private DialogUI dialogUI;
        
        private DialogData currentDialog;
        private int currentLineIndex;
        private bool isDialogActive;
        private float dialogStartTime;
        private float advanceDelay = 0.2f;
        
        public event Action OnDialogStarted;
        public event Action OnDialogEnded;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            if (dialogUI == null)
            {
                dialogUI = FindFirstObjectByType<DialogUI>();
            }
        }

        public void StartDialog(DialogData dialogData)
        {
            if (dialogData == null || dialogData.LineCount == 0)
            {
                return;
            }

            currentDialog = dialogData;
            currentLineIndex = 0;
            isDialogActive = true;
            dialogStartTime = Time.time;
            
            if (dialogUI != null)
            {
                dialogUI.ShowPanel();
                
                CancelInvoke(nameof(DisplayCurrentLine));
                Invoke(nameof(DisplayCurrentLine), 0.05f);
            }
            
            OnDialogStarted?.Invoke();
        }

        public void StartDialog(DialogData dialogData, int index)
        {
            if (dialogData == null || dialogData.LineCount == 0)
            {
                return;
            }

            currentDialog = dialogData;
            if (index < currentDialog.LineCount && index >= 0)
                currentLineIndex = index;
            else
                currentLineIndex = 0;

            isDialogActive = true;
            dialogStartTime = Time.time;

            if (dialogUI != null)
            {
                dialogUI.ShowPanel();

                CancelInvoke(nameof(DisplayCurrentLine));
                Invoke(nameof(DisplayCurrentLine), 0.05f);
            }

            OnDialogStarted?.Invoke();
        }

        public bool AdvanceDialog()
        {
            if (!isDialogActive)
            {
                return false;
            }

            if (Time.time < dialogStartTime + advanceDelay)
            {
                return false;
            }

            currentLineIndex++;

            if (currentLineIndex >= currentDialog.LineCount)
            {
                EndDialog();
                return true;
            }

            DisplayCurrentLine();

            return false;
        }

        public void EndDialog()
        {
            isDialogActive = false;
            dialogUI.HidePanel();
            OnDialogEnded?.Invoke();
        }

        private void DisplayCurrentLine()
        {
            DialogLine line = currentDialog.GetLineAt(currentLineIndex);
            dialogUI.UpdateUI(line.speakerName, line.speakerPortrait, line.message);
        }

        public bool IsDialogActive => isDialogActive;
    }
} 