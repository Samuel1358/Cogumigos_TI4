using UnityEngine;
using System;

/// <summary>
/// Interaction script para espelhos - herda de Interaction
/// </summary>
public class MirrorInteractionScript : Interaction
{
    private MirrorReflector _mirrorReflector;
    private Action<MirrorReflector> _mirrorAction;

    public override void Interact(Player player)
    {
        Debug.Log("Mirror Interact");
        _mirrorAction?.Invoke(_mirrorReflector);
    }

    public void Assign(MirrorReflector mirrorReflector, Action<MirrorReflector> mirrorAction)
    {
        this._mirrorReflector = mirrorReflector;
        this._mirrorAction = mirrorAction;
    }

    public void SetInteractJustOnce(bool interactOnce)
    {
        this._interactJustOnce = interactOnce;
    }
} 