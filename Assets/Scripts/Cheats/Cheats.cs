using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

public class Cheats : MonoBehaviour
{
    private CheatsInputs _inputActions;
    private CheatsInputs.CheatsActions _cheatsActions;
    
    // Fields
    private bool _infinityJump;
    private bool _imortal;

    public Action onShiftInfinityJump;
    public Action onShiftImortal;

    // Properties
    public bool InfinityJump { get { return _infinityJump; } }
    public bool Imortal { get { return _imortal; } }

    private void OnEnable()
    {
        _inputActions = new CheatsInputs();
        _cheatsActions = _inputActions.Cheats;
        _cheatsActions.Enable();

        _cheatsActions.Scene1.started += Scene1Action;
        _cheatsActions.Scene2.started += Scene2Action;
        _cheatsActions.Scene3.started += Scene3Action;
        //_cheatsActions.Scene4.started += Scene4Action;
        _cheatsActions.InfinityJump.started += ShiftInfinityJumpAction;
        _cheatsActions.Imortal.started += ShiftImortalAction;
    }

    private void OnDisable()
    {
        _cheatsActions.Scene1.started -= Scene1Action;
        _cheatsActions.Scene2.started -= Scene2Action;
        _cheatsActions.Scene3.started -= Scene3Action;
        //_cheatsActions.Scene4.started -= Scene4Action;
        _cheatsActions.InfinityJump.started -= ShiftInfinityJumpAction;
        _cheatsActions.Imortal.started -= ShiftImortalAction;

        _cheatsActions.Disable();
    }

    #region // Public Methods

    public void Scene1()
    {
        if (SceneManager.sceneCountInBuildSettings < 1)
            return;

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene(0);
    }

    public void Scene2()
    {
        if (SceneManager.sceneCountInBuildSettings < 2)
            return;

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene(1);
    }

    public void Scene3()
    {
        if (SceneManager.sceneCountInBuildSettings < 3)
            return;

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene(2);
    }

    public void Scene4()
    {
        if (SceneManager.sceneCountInBuildSettings < 4)
            return;

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene(3);
    }

    public void ShiftInfinityJump()
    {
        _infinityJump = !_infinityJump;
        onShiftInfinityJump.Invoke();
    }

    public void ShiftImortal()
    {
        _imortal = !_imortal;
        onShiftImortal.Invoke();
    }

    #endregion

    #region // Private Methods

    private void Scene1Action(CallbackContext callbackContext)
    {
        Scene1();
    }

    private void Scene2Action(CallbackContext callbackContext)
    {
        Scene2();
    }

    private void Scene3Action(CallbackContext callbackContext)
    {
        Scene3();
    }

    private void Scene4Action(CallbackContext callbackContext)
    {
        Scene4();
    }

    private void ShiftInfinityJumpAction(CallbackContext callbackContext)
    {
        ShiftInfinityJump();
    }

    private void ShiftImortalAction(CallbackContext callbackContext)
    {
        ShiftImortal();
    }

    #endregion
}
