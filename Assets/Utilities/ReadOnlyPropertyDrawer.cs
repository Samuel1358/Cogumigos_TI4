using UnityEngine;
#if UNITY_EDITOR 
using UnityEditor;

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyPropertyDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        GUI.enabled = false;

        EditorGUI.PropertyField(position, property, label);

        GUI.enabled = true;
    }
}
#endif
