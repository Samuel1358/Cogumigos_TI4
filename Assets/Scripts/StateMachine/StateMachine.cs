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

        _currentState.Enter();
        Debug.Log("Entring: " + _currentState);
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
    public void OntriggerEnter(Collider collider) {
        _currentState?.OnTriggerEnter(collider);
    }public void OntriggerExit(Collider collider) {
        _currentState?.OnTriggerExit(collider);
    }
}
