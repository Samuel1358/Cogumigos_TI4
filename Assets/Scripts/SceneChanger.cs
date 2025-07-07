using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void ChangeScene(int sceneIndex)
    {
        DOTween.KillAll();
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
    }

    public void ChangeScene(string sceneName)
    {
        DOTween.KillAll();
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
