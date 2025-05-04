using UnityEngine;

public class TEST_CoguAssignPoint : MonoBehaviour
{
    [SerializeField] private TEST_CoguCastter _coguCastter;

    // Public Methods
    public void AssingWildCogu(TEST_WildCogu wildCogu)
    {
        _coguCastter.CoguCount++;
        wildCogu.SelfDestruction();
    }
}
