using UnityEngine;
using UnityEngine.Events;

public class Lever : Switch
{
    [SerializeField] private Switchable _switchable;
    [SerializeField] private InteractingArea _area;
    [SerializeField] private Animator _leverAnimator;
    [SerializeField] private bool _interactJustOnce = true;
    private LeverInteraction _interaction;
    private bool _once = false;

    [Space]

    [SerializeField] private UnityEvent _onInteract;

    private void Awake()
    {
        _interaction = ScriptableObject.CreateInstance<LeverInteraction>();
        _interaction.Assign(_switchable, Activate);

        // NullReferenceException
        //Debug.Log(_area);
        //Debug.Log(_interaction);
        _area.Assign(_interaction);

        //UnityAction testeAction = () => Debug.Log("");
        //_onInteract.AddListener(testeAction);
    }

    // Inherit Methods
    protected override void Activate(Switchable obj) 
    {
        Debug.Log("LEVER - " + _interaction);
        Debug.Log("LEVER - " + _area._interaction);
        if ((_interactJustOnce) ? _once : false)
            return;

        Debug.Log("LEVER - pass");
        if (_switchable != null){
            _switchable.Activate();
            _once = true;
            _leverAnimator.SetTrigger("ChengeActivate");
            GameIniciator.Instance.AudioManagerInstance.PlaySFX(SoundEffectNames.LEVER);

            Debug.Log("LEVER - active");
        }

        _onInteract?.Invoke();
    }

    protected override void Disable(Switchable obj) { }
}
