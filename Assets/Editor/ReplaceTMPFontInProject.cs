using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEditor.SceneManagement;
using System.IO;

public class ReplaceTMPFontInProject : EditorWindow
{
    public TMP_FontAsset newFont;

    [MenuItem("Tools/Replace TMP Font In Project")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ReplaceTMPFontInProject), false, "Replace TMP Font");
    }

    void OnGUI()
    {
        GUILayout.Label("Replace TMP_FontAsset in all Prefabs and Scenes", EditorStyles.boldLabel);
        newFont = (TMP_FontAsset)EditorGUILayout.ObjectField("New Font Asset", newFont, typeof(TMP_FontAsset), false);

        if (GUILayout.Button("Replace Fonts"))
        {
            if (newFont == null)
            {
                EditorUtility.DisplayDialog("Error", "Please assign a TMP Font Asset before running.", "OK");
                return;
            }

            ReplaceInPrefabs();
            ReplaceInScenes();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("Done", "Font replacement completed!", "OK");
        }
    }

    void ReplaceInPrefabs()
    {
        string[] prefabGUIDs = AssetDatabase.FindAssets("t:Prefab");

        foreach (string guid in prefabGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            bool changed = false;
            TMP_Text[] texts = prefab.GetComponentsInChildren<TMP_Text>(true);
            foreach (var text in texts)
            {
                if (text != null && text.font != newFont)
                {
                    text.font = newFont;
                    EditorUtility.SetDirty(text);
                    changed = true;
                }
            }

            if (changed)
            {
                EditorUtility.SetDirty(prefab);
                PrefabUtility.SavePrefabAsset(prefab);
            }
        }
    }

    void ReplaceInScenes()
    {
        string[] sceneGUIDs = AssetDatabase.FindAssets("t:Scene");

        foreach (string guid in sceneGUIDs)
        {
            string scenePath = AssetDatabase.GUIDToAssetPath(guid);
            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

            bool changed = false;
            TMP_Text[] texts = Object.FindObjectsByType<TMP_Text>(FindObjectsSortMode.None);
            
            foreach (var text in texts)
            {
                if (text != null && text.font != newFont)
                {
                    text.font = newFont;
                    EditorUtility.SetDirty(text);
                    changed = true;
                }
            }

            if (changed)
            {
                EditorSceneManager.MarkSceneDirty(scene);
                EditorSceneManager.SaveScene(scene);
            }
        }
    }
}
