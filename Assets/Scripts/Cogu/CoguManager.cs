using System.Collections.Generic;
using UnityEngine;

public class CoguManager : MonoBehaviour {

    // Fields
    [SerializeField] private List<CastCoguData> _coguVariants = new List<CastCoguData>();

    private Dictionary<CoguType, Cogu> _coguDictionary = new Dictionary<CoguType, Cogu>();
    private List<WildCogu> _wildCoguList = new List<WildCogu>();
    public List<Cogu> _coguList = new List<Cogu>();

    // Properties
    public List<CastCoguData> CoguVariants { get { return _coguVariants; } }

    private void Awake() {

        foreach (CastCoguData castData in _coguVariants) {
            _coguDictionary.Add(castData.type, castData.cogu);
        }
    }

    private void Start() {
        UpdateWildCoguList();
    }

    private void FixedUpdate() {
        foreach (WildCogu wildCogu in _wildCoguList) {
            wildCogu.StateMachine.Update();
        }

        foreach (Cogu cogu in _coguList) {
            cogu.StateMachine.Update();
        }
    }

    // Get & Set
    public bool TryGetCoguVariant(CoguType type, out Cogu cogu) {
        if (_coguDictionary.TryGetValue(type, out cogu))
            return true;

        Debug.LogError($"'{type}' isn't a assigned key name to a existent cogu variant!");
        return false;
    }

    // Public Methods
    public void AssingWildCogu(WildCogu wildCogu) {
        _wildCoguList.Add(wildCogu);
    }

    public void RemoveWildCogu(WildCogu wildCogu) {
        _wildCoguList.Remove(wildCogu);
    }

    public void AssingCogu(Cogu cogu) {
        _coguList.Add(cogu);
    }

    public void RemoveCogu(Cogu cogu) {
        _coguList.Remove(cogu);
    }

    // Private Methods
    private void UpdateWildCoguList() {
        _wildCoguList.Clear();

        WildCogu[] aux = FindObjectsByType<WildCogu>(FindObjectsSortMode.None);

        foreach (WildCogu cogu in aux) {
            _wildCoguList.Add(cogu);
        }
    }

    private void UpdateCoguList() {
        _coguList.Clear();

        Cogu[] aux = FindObjectsByType<Cogu>(FindObjectsSortMode.None);

        foreach (Cogu cogu in aux) {
            _coguList.Add(cogu);
        }
    }
}
