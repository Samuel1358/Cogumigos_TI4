using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogSystem
{
    [CreateAssetMenu(fileName = "New Dialog", menuName = "Dialog System/Dialog Data")]
    public class DialogData : ScriptableObject
    {
        [SerializeField] private List<DialogLine> dialogLines = new List<DialogLine>();

        public int LineCount => dialogLines.Count;
        
        public DialogLine GetLineAt(int index)
        {
            if (index < 0 || index >= dialogLines.Count)
            {
                return dialogLines.Count > 0 ? dialogLines[0] : new DialogLine();
            }
            
            return dialogLines[index];
        }
    }

    [Serializable]
    public class DialogLine
    {
        public string speakerName;
        public Sprite speakerPortrait;
        [TextArea(3, 10)]
        public string message;
    }
} 