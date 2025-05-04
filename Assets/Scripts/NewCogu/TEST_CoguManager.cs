using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TEST_CoguManager : MonoBehaviour
{
    public static TEST_CoguManager instance;

    // Fields
    [SerializeField] private List<TEST_CastCoguData> _coguVariants = new List<TEST_CastCoguData>();

    private List<TEST_WildCogu> _wildCoguList = new List<TEST_WildCogu>();

    // Properties
    public List<TEST_CastCoguData> CoguVariants {  get { return _coguVariants; } }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        UpdateList();
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < _wildCoguList.Count; i++)
        {
            _wildCoguList[i].StateMachine.Update();
        }
    }

    // Private Methods
    public void AssingWildCogu(TEST_WildCogu wildCogu)
    {
        _wildCoguList.Add(wildCogu);
    }

    public void RemoveWildCogu(TEST_WildCogu wildCogu)
    {
        _wildCoguList.Remove(wildCogu);
    }

    private void UpdateList()
    {
        _wildCoguList.Clear();

        TEST_WildCogu[] aux = FindObjectsByType<TEST_WildCogu>(FindObjectsSortMode.None);

        foreach (TEST_WildCogu cogu in aux)
        {
            _wildCoguList.Add(cogu);
        }
    }

}
