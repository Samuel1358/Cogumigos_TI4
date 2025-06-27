using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DialogSystem
{
    public class DialogUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject dialogPanel;
        [SerializeField] private Image speakerPortrait;
        [SerializeField] private TextMeshProUGUI speakerNameText;
        [SerializeField] private TextMeshProUGUI messageText;

        [Header("Animation Settings")]
        [SerializeField] private float textSpeed = 0.05f;
        
        private bool isTextAnimating;
        private string currentMessage;
        private float textTimer;
        private int currentCharIndex;

        private void Awake()
        {
            if (dialogPanel != null)
            {
                dialogPanel.SetActive(false);
            }
        }

        private void Update()
        {
            if (isTextAnimating)
            {
                textTimer += Time.deltaTime;
                
                if (textTimer >= textSpeed)
                {
                    textTimer = 0f;
                    currentCharIndex++;
                    
                    if (currentCharIndex > currentMessage.Length)
                    {
                        isTextAnimating = false;
                        return;
                    }
                    
                    messageText.text = currentMessage.Substring(0, currentCharIndex);
                }
            }
        }

        public void ShowPanel()
        {
            if (dialogPanel != null)
            {
                Canvas rootCanvas = GetComponentInParent<Canvas>();
                if (rootCanvas != null && !rootCanvas.gameObject.activeSelf)
                {
                    rootCanvas.gameObject.SetActive(true);
                }
                
                dialogPanel.SetActive(true);
                
                if (speakerNameText != null)
                    speakerNameText.gameObject.SetActive(true);
                
                if (messageText != null)
                    messageText.gameObject.SetActive(true);
                
                if (speakerPortrait != null)
                    speakerPortrait.gameObject.SetActive(true);
            }
        }

        public void HidePanel()
        {
            if (dialogPanel != null)
            {
                dialogPanel.SetActive(false);
            }
        }

        public void UpdateUI(string speakerName, Sprite portrait, string message)
        {
            if (speakerNameText != null)
            {
                speakerNameText.text = speakerName;
            }
            
            if (speakerPortrait != null)
            {
                speakerPortrait.sprite = portrait;
                speakerPortrait.gameObject.SetActive(portrait != null);
            }
            
            if (messageText != null)
            {
                StartTextAnimation(message);
            }
        }

        public void CompleteTextAnimation()
        {
            if (isTextAnimating)
            {
                isTextAnimating = false;
                messageText.text = currentMessage;
            }
        }
        
        private void StartTextAnimation(string message)
        {
            currentMessage = message;
            currentCharIndex = 0;
            messageText.text = "";
            isTextAnimating = true;
            textTimer = 0f;
        }
    }
} 