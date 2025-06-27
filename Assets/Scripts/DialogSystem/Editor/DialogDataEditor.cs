using UnityEngine;
using UnityEditor;

namespace DialogSystem.Editor
{
    [CustomEditor(typeof(DialogData))]
    public class DialogDataEditor : UnityEditor.Editor
    {
        private GUIStyle headerStyle;
        private SerializedProperty _dialogLinesProp;
        private SerializedProperty _durationProp;
        private SerializedProperty _showJustOnceProp;
        private SerializedProperty _randomLineProp;
        
        private void OnEnable()
        {           
            headerStyle = new GUIStyle();
            headerStyle.fontStyle = FontStyle.Bold;
            headerStyle.fontSize = 14;
            headerStyle.normal.textColor = Color.white;

            _dialogLinesProp = serializedObject.FindProperty("_dialogLines");
            _durationProp = serializedObject.FindProperty("_duration");
            _showJustOnceProp = serializedObject.FindProperty("_showJustOnce");
            _randomLineProp = serializedObject.FindProperty("_randomLine");
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.LabelField("Dialog Editor", headerStyle);

            EditorGUILayout.Space();

            SettingsData();

            EditorGUILayout.Space();

            int dialogCount = _dialogLinesProp.arraySize;
            EditorGUILayout.LabelField($"Dialog Lines: {dialogCount}");
            
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Dialog Line"))
            {
                AddDialogLine();
            }
            
            if (GUILayout.Button("Clear All Lines"))
            {
                if (EditorUtility.DisplayDialog("Confirm Delete", 
                    "Are you sure you want to delete all dialog lines?", "Yes", "No"))
                {
                    _dialogLinesProp.ClearArray();
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space(10);
            
            // Draw dialog lines
            for (int i = 0; i < _dialogLinesProp.arraySize; i++)
            {
                DrawDialogLineEditor(i);
            }
            
            serializedObject.ApplyModifiedProperties();
        }

        private void SettingsData()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_durationProp);
            GUILayout.BeginHorizontal();

            EditorGUILayout.PropertyField(_showJustOnceProp);
            EditorGUILayout.PropertyField(_randomLineProp);

            GUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }
        
        private void DrawDialogLineEditor(int index)
        {
            SerializedProperty dialogLine = _dialogLinesProp.GetArrayElementAtIndex(index);
            SerializedProperty speakerName = dialogLine.FindPropertyRelative("speakerName");
            SerializedProperty speakerPortrait = dialogLine.FindPropertyRelative("speakerPortrait");
            SerializedProperty message = dialogLine.FindPropertyRelative("message");
            
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Dialog Line {index + 1}", EditorStyles.boldLabel);
            
            // Add buttons to move up, move down, and remove this dialog line
            if (GUILayout.Button("↑", GUILayout.Width(25)))
            {
                if (index > 0)
                {
                    _dialogLinesProp.MoveArrayElement(index, index - 1);
                }
            }
            
            if (GUILayout.Button("↓", GUILayout.Width(25)))
            {
                if (index < _dialogLinesProp.arraySize - 1)
                {
                    _dialogLinesProp.MoveArrayElement(index, index + 1);
                }
            }
            
            if (GUILayout.Button("X", GUILayout.Width(25)))
            {
                _dialogLinesProp.DeleteArrayElementAtIndex(index);
                return;
            }
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.PropertyField(speakerName, new GUIContent("Speaker Name"));
            EditorGUILayout.PropertyField(speakerPortrait, new GUIContent("Speaker Portrait"));
            EditorGUILayout.PropertyField(message, new GUIContent("Message"));
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(5);
        }
        
        private void AddDialogLine()
        {
            int index = _dialogLinesProp.arraySize;
            _dialogLinesProp.InsertArrayElementAtIndex(index);
            
            SerializedProperty newLine = _dialogLinesProp.GetArrayElementAtIndex(index);
            newLine.FindPropertyRelative("speakerName").stringValue = "";
            newLine.FindPropertyRelative("speakerPortrait").objectReferenceValue = null;
            newLine.FindPropertyRelative("message").stringValue = "";
        }
    }
} 