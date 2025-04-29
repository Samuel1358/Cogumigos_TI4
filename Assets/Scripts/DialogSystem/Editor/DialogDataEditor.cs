using UnityEngine;
using UnityEditor;

namespace DialogSystem.Editor
{
    [CustomEditor(typeof(DialogData))]
    public class DialogDataEditor : UnityEditor.Editor
    {
        private SerializedProperty dialogLinesProperty;
        private GUIStyle headerStyle;
        
        private void OnEnable()
        {
            dialogLinesProperty = serializedObject.FindProperty("dialogLines");
            
            headerStyle = new GUIStyle();
            headerStyle.fontStyle = FontStyle.Bold;
            headerStyle.fontSize = 14;
            headerStyle.normal.textColor = Color.white;
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.LabelField("Dialog Editor", headerStyle);
            EditorGUILayout.Space(10);
            
            int dialogCount = dialogLinesProperty.arraySize;
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
                    dialogLinesProperty.ClearArray();
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space(10);
            
            // Draw dialog lines
            for (int i = 0; i < dialogLinesProperty.arraySize; i++)
            {
                DrawDialogLineEditor(i);
            }
            
            serializedObject.ApplyModifiedProperties();
        }
        
        private void DrawDialogLineEditor(int index)
        {
            SerializedProperty dialogLine = dialogLinesProperty.GetArrayElementAtIndex(index);
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
                    dialogLinesProperty.MoveArrayElement(index, index - 1);
                }
            }
            
            if (GUILayout.Button("↓", GUILayout.Width(25)))
            {
                if (index < dialogLinesProperty.arraySize - 1)
                {
                    dialogLinesProperty.MoveArrayElement(index, index + 1);
                }
            }
            
            if (GUILayout.Button("X", GUILayout.Width(25)))
            {
                dialogLinesProperty.DeleteArrayElementAtIndex(index);
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
            int index = dialogLinesProperty.arraySize;
            dialogLinesProperty.InsertArrayElementAtIndex(index);
            
            SerializedProperty newLine = dialogLinesProperty.GetArrayElementAtIndex(index);
            newLine.FindPropertyRelative("speakerName").stringValue = "";
            newLine.FindPropertyRelative("speakerPortrait").objectReferenceValue = null;
            newLine.FindPropertyRelative("message").stringValue = "";
        }
    }
} 