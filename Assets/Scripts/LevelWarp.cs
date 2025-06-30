using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class LevelWarp : MonoBehaviour
{
    [SerializeField] private int _targetSceneIndex;
    [SerializeField] private UnityEvent _onJustBeforeWarp;

    private void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        _onJustBeforeWarp.Invoke();
        SceneManager.LoadScene(_targetSceneIndex, LoadSceneMode.Single);
    }
}
