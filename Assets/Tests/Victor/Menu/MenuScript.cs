using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {
    public void LoadScene(int sceneIndex) {
        SceneManager.LoadScene(sceneIndex);
    }
    public void ExitGame() {
        Application.Quit();
    }
}
