using UnityEngine;
using UnityEngine.Events;

public class Lever : Switch
{
    [SerializeField] private Switchable _switchable;
    [SerializeField] private InteractingArea _area;
    [SerializeField] private Animator _leverAnimator;
    private LeverInteraction _interaction;
    private bool _once = false;

    [Space]

    [SerializeField] private UnityEvent _onInteract;

    private void Awake()
    {
        _interaction = ScriptableObject.CreateInstance<LeverInteraction>();
        _interaction.Assign(_switchable, Activate);

        // NullReferenceException
        //Debug.Log(_interaction);
        _area.Assign(_interaction);

        //UnityAction testeAction = () => Debug.Log("");
        //_onInteract.AddListener(testeAction);
    }

    // Inherit Methods
    protected override void Activate(Switchable obj) 
    {
        if (_once)
            return;

        if (_switchable != null){
            _switchable.Activate();
            _once = true;
            _leverAnimator.SetTrigger("ChengeActivate");
            GameIniciator.Instance.AudioManagerInstance.PlaySFX(SoundEffectNames.LEVER);

            //Debug.Log("LEVER - active");
        }

        _onInteract?.Invoke();
    }

    protected override void Disable(Switchable obj) { }
}
