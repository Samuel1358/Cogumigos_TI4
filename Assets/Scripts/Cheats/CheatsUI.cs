using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;
using TMPro;

public class CheatsUI : MonoBehaviour
{
    private CheatsInputs _inputActions;
    private CheatsInputs.UIActions _uiActions;

    [SerializeField] private GameObject _painel;
    [SerializeField] private TMP_Text _sceneName;
    [SerializeField] private Toggle _infinityJumpToggle;
    [SerializeField] private Toggle _imortalToggle;

    private void OnEnable()
    {
        _inputActions = new CheatsInputs();
        _uiActions = _inputActions.UI;
        _uiActions.Enable();

        _uiActions.OpenClose.started += OpenCloseAction;
    }

    private void OnDisable()
    {
        _uiActions.OpenClose.started -= OpenCloseAction;

        _uiActions.Disable();
    }

    private void Start()
    {
        Cheats.instance.onShiftInfinityJump += OnShiftInfinityJump;
        Cheats.instance.onShiftImortal += OnShiftImortal;
    }

    // Input
    private void OpenCloseAction(CallbackContext callbackContext)
    {
        _infinityJumpToggle.onValueChanged.RemoveListener(OnToggleInfinityJump);
        _imortalToggle.onValueChanged.RemoveListener(OnToggleImortal);

        _sceneName.text = SceneManager.GetActiveScene().name;
        _infinityJumpToggle.isOn = Cheats.instance.InfinityJump;
        _imortalToggle.isOn = Cheats.instance.Imortal;

        _infinityJumpToggle.onValueChanged.AddListener(OnToggleInfinityJump);
        _imortalToggle.onValueChanged.AddListener(OnToggleImortal);

        _painel.SetActive(!_painel.activeSelf);
    }

    // Private Methods
    private void OnShiftInfinityJump()
    {
        _infinityJumpToggle.onValueChanged.RemoveListener(OnToggleInfinityJump);

        _infinityJumpToggle.isOn = Cheats.instance.InfinityJump;

        _infinityJumpToggle.onValueChanged.AddListener(OnToggleInfinityJump);
    }

    private void OnShiftImortal()
    {
        _imortalToggle.onValueChanged.RemoveListener(OnToggleImortal);

        _imortalToggle.isOn = Cheats.instance.Imortal;

        _imortalToggle.onValueChanged.AddListener(OnToggleImortal);
    }

    private void OnToggleInfinityJump(bool value)
    {
        Cheats.instance.ShiftInfinityJump();
    }

    private void OnToggleImortal(bool value)
    {
        Cheats.instance.ShiftImortal();
    }
}
