using System;
using UnityEngine;

public class LightInteraction : Interaction
{
    // Fields
    private Light _light;
    private Action<Light> _lightAction;

    public override void Interact(Player player)
    {
        Debug.Log("LightInteract");
        _lightAction?.Invoke(_light);
    }

    public void Assign(Light light, Action<Light> lightAction)
    {
        this._light = light;
        this._lightAction = lightAction;
    }

    public void SetInteractJustOnce(bool interactOnce)
    {
        this._interactJustOnce = interactOnce;
    }
} 