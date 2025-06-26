using UnityEngine;
using UnityEngine.Events;

public class Lever : Switch
{
    [SerializeField] private Switchable _switchable;
    [SerializeField] private InteractingArea _area;
    private LeverInteraction _interaction;

    [Space]

    [SerializeField] private UnityEvent _onInteract;

    private void Awake()
    {
        _interaction = ScriptableObject.CreateInstance<LeverInteraction>();
        _interaction.Assign(_switchable, Activate);

        _area.Assign(_interaction);

        UnityAction testeAction = () => Debug.Log("");
        _onInteract.AddListener(testeAction);
    }

    // Inherit Methods
    protected override void Activate(Switchable obj) 
    {
        if (_switchable != null)
            _switchable.Activate();

        _onInteract?.Invoke();
    }

    protected override void Disable(Switchable obj) { }
}
