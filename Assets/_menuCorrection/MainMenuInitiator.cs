using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class MainMenuInitiator : MonoBehaviour {
    public void LoadScene(int sceneIndex) {
        GameIniciator.Instance.LoadScenes(sceneIndex);
    }

    public void ExitGame() {

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
