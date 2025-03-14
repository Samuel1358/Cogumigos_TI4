using System;
using System.Collections.Generic;
using UnityEngine;

public enum CoguStatesDebugType
{
    Enter,
    Exit,
    HandleInput,
    Update,
    PhysicsUpdate,
    OnAnimationEnterEvent,
    OnAnimationExitEvent,
    OnAnimationTransitionEvent,
    OnTriggerEnter,
    OnTriggerExit,
}
public enum CoguStatesDebugMode
{
    Label,
    Console,
}

[RequireComponent(typeof(Cogu))]
public class CoguStatesDebug : MonoBehaviour
{
    [SerializeField] private CoguStatesDebugType debugType;
    [SerializeField] private CoguStatesDebugMode debugMode;

    [Header("Debug label")]
    [SerializeField] private string _;


    private Cogu cogu;
    private Dictionary<CoguState, string> states = new Dictionary<CoguState, string>();

    private void Start()
    {
        cogu = GetComponent<Cogu>();

        CoguState[] list = new CoguState[] {
            cogu.stateMachine.idleState,
            cogu.stateMachine.attractState,
            cogu.stateMachine.followState,
            cogu.stateMachine.throwState
        };

        Action debug = null;
        switch (debugMode)
        {
            case CoguStatesDebugMode.Label:
                debug = LabelDebug;
                break;
            case CoguStatesDebugMode.Console:
                debug = ConsoleDebug;
                break;
            default:
                debug = LabelDebug;
                break;
        }

        foreach (CoguState state in list)
        {
            states.Add(state, state.name);

            switch (debugType)
            {
                case CoguStatesDebugType.Enter:
                    state.enter += debug;
                    break;

                case CoguStatesDebugType.Exit:
                    state.exit += debug;
                    break;

                case CoguStatesDebugType.HandleInput:
                    state.handleInput += debug;
                    break;

                case CoguStatesDebugType.Update:
                    state.update += debug;
                    break;

                case CoguStatesDebugType.PhysicsUpdate:
                    state.physicsUpdate += debug;
                    break;

                case CoguStatesDebugType.OnAnimationEnterEvent:
                    state.onAnimationEnterEvent += debug;
                    break;

                case CoguStatesDebugType.OnAnimationExitEvent:
                    state.onAnimationExitEvent += debug;
                    break;

                case CoguStatesDebugType.OnAnimationTransitionEvent:
                    state.onAnimationTransitionEvent += debug;
                    break;

                case CoguStatesDebugType.OnTriggerEnter:
                    state.onTriggerEnter += debug;
                    break;

                case CoguStatesDebugType.OnTriggerExit:
                    state.onTriggerExit += debug;
                    break;
            }
        }

        debug.Invoke();
    }

    // Private Methods
    private void ConsoleDebug()
    {
        Debug.Log(states[(CoguState)cogu.stateMachine.GetCurrentState()]);
    }

    private void LabelDebug()
    {
        this._ = states[(CoguState)cogu.stateMachine.GetCurrentState()];
    }
}
