using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
namespace Victor {
    public abstract class StateMachine {
        protected IState _currentState;

        public void ChangeState(IState newState) {
            _currentState?.Exit();

            _currentState = newState;

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
    }
}
