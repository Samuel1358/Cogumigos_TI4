using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TEST_CoguManager : MonoBehaviour
{
    public static TEST_CoguManager instance;

    // Fields
    [SerializeField] private List<TEST_CastCoguData> _coguVariants = new List<TEST_CastCoguData>();

    private Dictionary<string, TEST_Cogu> _coguDictionary;
    private List<TEST_WildCogu> _wildCoguList = new List<TEST_WildCogu>();

    // Properties
    public List<TEST_CastCoguData> CoguVariants {  get { return _coguVariants; } }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        foreach (TEST_CastCoguData castData in _coguVariants)
        {
            _coguDictionary.Add(castData.keyName, castData.cogu);
        }
    }

    private void Start()
    {
        UpdateWildCoguList();
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < _wildCoguList.Count; i++)
        {
            _wildCoguList[i].StateMachine.Update();
        }
    }

    // Get & Set
    public TEST_Cogu GetCoguVariant(string keyName)
    {
        if (_coguDictionary.TryGetValue(keyName, out TEST_Cogu cogu))
            return cogu;

        Debug.LogWarning($"{keyName} isn't a assigned key name to a existent cogu variant!");
        return null;
    }

    // Public Methods
    public void AssingWildCogu(TEST_WildCogu wildCogu)
    {
        _wildCoguList.Add(wildCogu);
    }

    public void RemoveWildCogu(TEST_WildCogu wildCogu)
    {
        _wildCoguList.Remove(wildCogu);
    }

    // Private Methods
    private void UpdateWildCoguList()
    {
        _wildCoguList.Clear();

        TEST_WildCogu[] aux = FindObjectsByType<TEST_WildCogu>(FindObjectsSortMode.None);

        foreach (TEST_WildCogu cogu in aux)
        {
            _wildCoguList.Add(cogu);
        }
    }

}
