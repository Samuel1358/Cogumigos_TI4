using System;
using UnityEngine;

public class InteractCoguState : CoguState
{
    private event Action _interacting;

    // Inerited Constructor
    public InteractCoguState(CoguStateMachine stateMachine) : base(stateMachine) { }

    // Inherited Public Methods
    public override void Enter()
    {
        //Debug.Log("Enter - Interact");
        _interacting += _stateMachine.Cogu.StartInteracting;
        _stateMachine.Cogu.ResetAnableCast();
        AudioManager.Instance.PlaySFX("CoguTransform");

        _stateMachine.Cogu.Animator.SetBool("Interact", true);
    }

    public void Interact()
    {
        _interacting?.Invoke();
    }

    public void EndInteracting(Action act)
    {
        _interacting -= act;
        _stateMachine.Cogu.SelfDestruction();
    }

    // DEBUG - deletar
    private void UpdateDebug()
    {
        Debug.Log("Update - Interact");
    }
}
