using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

public class ReplaceMaterialInProject : EditorWindow
{
    public Material materialToReplace;
    public Material newMaterial;

    [MenuItem("Tools/Replace Material In Project")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ReplaceMaterialInProject), false, "Replace Materials");
    }

    void OnGUI()
    {
        GUILayout.Label("Replace Material In All Scenes", EditorStyles.boldLabel);

        materialToReplace = (Material)EditorGUILayout.ObjectField("Material to Replace", materialToReplace, typeof(Material), false);
        newMaterial = (Material)EditorGUILayout.ObjectField("New Material", newMaterial, typeof(Material), false);

        if (GUILayout.Button("Replace Materials"))
        {
            if (materialToReplace == null || newMaterial == null)
            {
                EditorUtility.DisplayDialog("Error", "Please assign both materials before running.", "OK");
                return;
            }

            ReplaceInAllScenes();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog("Done", "Material replacement completed!", "OK");
        }
    }

    void ReplaceInAllScenes()
    {
        string[] sceneGUIDs = AssetDatabase.FindAssets("t:Scene");

        foreach (string guid in sceneGUIDs)
        {
            string scenePath = AssetDatabase.GUIDToAssetPath(guid);
            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

            Renderer[] renderers = Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None);

            bool sceneChanged = false;

            foreach (Renderer renderer in renderers)
            {
                bool materialChanged = false;
                Material[] materials = renderer.sharedMaterials;

                for (int i = 0; i < materials.Length; i++)
                {
                    if (materials[i] == materialToReplace)
                    {
                        materials[i] = newMaterial;
                        materialChanged = true;
                    }
                }

                if (materialChanged)
                {
                    renderer.sharedMaterials = materials;
                    EditorUtility.SetDirty(renderer);
                    sceneChanged = true;
                }
            }

            if (sceneChanged)
            {
                EditorSceneManager.MarkSceneDirty(scene);
                EditorSceneManager.SaveScene(scene);
            }
        }
    }
}
