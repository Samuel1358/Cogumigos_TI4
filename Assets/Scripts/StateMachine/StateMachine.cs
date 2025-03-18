using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
public abstract class StateMachine {
    protected IState _currentState;

    public void ChangeState(IState newState) {
        _currentState?.Exit();
        Debug.Log("Exiting: " + _currentState);

        _currentState = newState;

        Debug.Log("Entring: " + _currentState);

        _currentState.Enter();
    }
    public void HandleInput() {
        _currentState?.HandleInput();
    }
    public void Update() {
        _currentState?.Update();
    }
    public void PhysicsUpdate() {
        _currentState?.PhysicsUpdate();
    }
    public void OnAnimationEnterEvent() {
        _currentState?.OnAnimationEnterEvent();
    }
    public void OnAnimationExitEvent() {
        _currentState?.OnAnimationExitEvent();
    }
    public void OnAnimationTransitionEvent() {
        _currentState?.OnAnimationTransitionEvent();
    }
}
