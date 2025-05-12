using System;
using UnityEngine;

public class LeverInteraction : Interaction
{
    // Fields
    private Switchable _switchable;
    private Action<Switchable> _leverAction;
    //private bool _once = true;

    public override void Interact(Player player)
    {
        Debug.Log("LeverInteract");
        _leverAction?.Invoke(_switchable);
    }

    public void Assign(Switchable switchable, Action<Switchable> leverAction)
    {
        this._switchable = switchable;
        this._leverAction = leverAction;
    }
}
