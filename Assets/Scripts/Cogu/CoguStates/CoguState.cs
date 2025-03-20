using System;
using UnityEditor;
using UnityEngine;

public abstract class CoguState : IState
{
    public string name { get; protected set; }
    protected CoguStateMachine stateMachine;

    // GoguStatesDebug
    public event Action enter;
    public event Action exit;
    public event Action handleInput;
    public event Action update;
    public event Action physicsUpdate;
    public event Action onAnimationEnterEvent;
    public event Action onAnimationExitEvent;
    public event Action onAnimationTransitionEvent;
    public event Action onTriggerEnter;
    public event Action onTriggerExit;

    // Constructor
    public CoguState(CoguStateMachine stateMachine, string name)
    {
        this.stateMachine = stateMachine;
        this.name = name;
    }

    // Interface Methods
    public virtual void Enter() { enter?.Invoke(); }

    public virtual void Exit() { exit?.Invoke(); }

    public virtual void HandleInput() { handleInput?.Invoke(); }

    public virtual void Update() { update?.Invoke(); }

    public virtual void PhysicsUpdate() { physicsUpdate?.Invoke(); }

    public virtual void OnAnimationEnterEvent() { onAnimationEnterEvent?.Invoke(); }

    public virtual void OnAnimationExitEvent() { onAnimationExitEvent?.Invoke(); }

    public virtual void OnAnimationTransitionEvent() { onAnimationTransitionEvent?.Invoke(); }

    public virtual void OnTriggerEnter(Collider collider) { onTriggerEnter?.Invoke(); }

    public virtual void OnTriggerExit(Collider collider) { onTriggerExit?.Invoke(); }
}
