using UnityEngine;

public class DisapearingObject : ResetableBase
{
    [SerializeField] private GameObject _object;

    private bool _activeAtCheckpoint;


    private void Start()
    {
        GameIniciator.Instance.RespawnControllerInstance.OnPlayerChangeCheckPoint += SetMemory;
    }

    private void SetMemory(Checkpoint checkpoint)
    {
        _activeAtCheckpoint = _object.activeSelf;
        Debug.Log("SetMemory");
    }

    public override void ResetObject()
    {
        _object.SetActive(_activeAtCheckpoint);
        Debug.Log("ResetObject");
    }
}
