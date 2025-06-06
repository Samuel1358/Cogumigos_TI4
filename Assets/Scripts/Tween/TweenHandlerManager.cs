using UnityEngine;

//[CreateAssetMenu(fileName = "TweenHandlerManager", menuName = "Scriptable Objects/TweensManager")]
public class TweenHandlerManager : ScriptableObject
{
    public static TweenHandlerManager instance;

    [SerializeField] private float _shakeStrenght;
    [SerializeField] private int _shakeVibrato;
    [SerializeField, Range(0f, 90f)] private float _randomness;

    private void OnValidate()
    {
        UpdateTweenHandlerData();
    }

    private void OnEnable()
    {
        UpdateValues();
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;

        UpdateValues();
    }

    private void UpdateValues()
    {
        _shakeStrenght = TweenHandler.shakeStrenght;
        _shakeVibrato = TweenHandler.shakeVibrato;
        _randomness = TweenHandler.randomness;
    }

    private void UpdateTweenHandlerData()
    {
        TweenHandler.shakeStrenght = _shakeStrenght;
        TweenHandler.shakeVibrato = _shakeVibrato;
        TweenHandler.randomness = _randomness;
    }
}
