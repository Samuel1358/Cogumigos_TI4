using System;
using UnityEngine;

public class ActivateSwitchInteraction : Interaction
{
    // Fields
    private ActivateSwitch _activateSwitch;
    private Action<ActivateSwitch> _switchAction;

    public override void Interact(Player player)
    {
        Debug.Log("ActivateSwitch Interact");
        _switchAction?.Invoke(_activateSwitch);
    }

    public void Assign(ActivateSwitch activateSwitch, Action<ActivateSwitch> switchAction)
    {
        this._activateSwitch = activateSwitch;
        this._switchAction = switchAction;
    }

    public void SetInteractJustOnce(bool interactOnce)
    {
        this._interactJustOnce = interactOnce;
    }
} 