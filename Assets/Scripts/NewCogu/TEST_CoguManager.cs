using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TEST_CoguManager : MonoBehaviour
{
    public static TEST_CoguManager instance;

    // Fields
    [SerializeField] private List<TEST_CastCoguData> _coguVariants = new List<TEST_CastCoguData>();

    private Dictionary<string, TEST_Cogu> _coguDictionary = new Dictionary<string, TEST_Cogu>();
    private List<TEST_WildCogu> _wildCoguList = new List<TEST_WildCogu>();
    private List<TEST_Cogu> _coguList = new List<TEST_Cogu>();

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
        foreach (TEST_WildCogu wildCogu in _wildCoguList)
        {
            wildCogu.StateMachine.Update();
        }

        foreach (TEST_Cogu cogu in _coguList)
        {
            cogu.StateMachine.Update();
        }
    }

    // Get & Set
    public bool TryGetCoguVariant(string keyName, out TEST_Cogu cogu)
    {
        if (_coguDictionary.TryGetValue(keyName, out cogu))
            return true;

        Debug.LogError($"'{keyName}' isn't a assigned key name to a existent cogu variant!");
        return false;
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

    public void AssingCogu(TEST_Cogu cogu)
    {
        _coguList.Add(cogu);
    }

    public void RemoveCogu(TEST_Cogu cogu)
    {
        _coguList.Remove(cogu);
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

    private void UpdateCoguList()
    {
        _coguList.Clear();

        TEST_Cogu[] aux = FindObjectsByType<TEST_Cogu>(FindObjectsSortMode.None);

        foreach (TEST_Cogu cogu in aux)
        {
            _coguList.Add(cogu);
        }
    }

}
