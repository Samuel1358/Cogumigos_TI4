using UnityEngine;

public class CoguCastPoint : MonoBehaviour
{
    [SerializeField] private CoguCastter _coguCastter;

    // Public Methods
    public void AssingWildCogu(WildCogu wildCogu)
    {
        _coguCastter.CoguCount++;
        Destroy(wildCogu.gameObject);
    }
}
