using UnityEngine;

public class Lever : Switch
{
    [SerializeField] private Switchable _switchable;
    [SerializeField] private InteractingArea _area;
    private LeverInteraction _interaction;

    private void Awake()
    {
        _interaction = ScriptableObject.CreateInstance<LeverInteraction>();
        _interaction.Assign(_switchable, Activate);

        _area.Assign(_interaction);
    }

    // Inherit Methods
    protected override void Activate(Switchable obj) 
    {
        _switchable.Activate();
    }

    protected override void Disable(Switchable obj) { }
}
