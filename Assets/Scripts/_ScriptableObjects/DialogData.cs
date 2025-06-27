using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogSystem
{
    [CreateAssetMenu(fileName = "New Dialog", menuName = "Dialog System/Dialog Data")]
    public class DialogData : ScriptableObject
    {
        // Fields
        [SerializeField] private List<DialogLine> _dialogLines = new List<DialogLine>();
        [SerializeField] private float _duration = 0f;
        [SerializeField] private bool _showJustOnce = false;
        [SerializeField] private bool _randomLine = false;

        // Properties
        public int LineCount => _dialogLines.Count;
        public float Duration => _duration;
        public bool ShowJustOnce => _showJustOnce;
        public bool RandomLine => _randomLine;
        
        public DialogLine GetLineAt(int index)
        {
            if (index < 0 || index >= _dialogLines.Count)
            {
                return _dialogLines.Count > 0 ? _dialogLines[0] : new DialogLine();
            }
            
            return _dialogLines[index];
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