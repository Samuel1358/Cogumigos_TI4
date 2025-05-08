using UnityEngine;
using UnityEngine.SceneManagement;

public class Cheats : MonoBehaviour
{
    public static Cheats instance;

    [SerializeField] private Player _player;
    [HideInInspector] private bool _infinityJump;
    [HideInInspector] public bool imortal;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Update()
    {
        if (_infinityJump)
            _player.GetStateMachine().ReusableData.CanDoubleJump = true;

        if (Input.GetKeyDown(KeyCode.F1))
            SceneManager.LoadScene(0);
        if (Input.GetKeyDown(KeyCode.F2))
            SceneManager.LoadScene(1);
        if (Input.GetKeyDown(KeyCode.F3))
            SceneManager.LoadScene(2);
        if (Input.GetKeyDown(KeyCode.F4))
            SceneManager.LoadScene(3);
        if (Input.GetKeyDown(KeyCode.F5))
            SceneManager.LoadScene(4);
    }

    // Get & Set
    public void SetInfinityJump(bool value)
    {
        _infinityJump = value;
    }

    public void SetImortal(bool value)
    {
        imortal = value;
    }
}
