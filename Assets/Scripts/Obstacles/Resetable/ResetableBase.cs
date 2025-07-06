using System.Collections;
using UnityEngine;

public abstract class ResetableBase : MonoBehaviour, IResetable
{
    [SerializeField] private Checkpoint _linkedCheckpoint;

    private bool _needReset;
    protected bool NeedReset
    {
        get { return _needReset; }
        set 
        { 
            if (_needReset != value)
            {
                _needReset = value;
            }
        }
    }

    protected virtual void OnEnable() 
    {
        StartCoroutine(ActivateResetable());
    }

    private void OnDisable() {
        GameIniciator.Instance.RespawnControllerInstance.OnPlayerChangeCheckPoint -= VerifyReset;
        GameIniciator.Instance.RespawnControllerInstance.TurnNonResetable(this);
    }

    private void VerifyReset(Checkpoint checkpoint) {
        if (GameIniciator.Instance.RespawnControllerInstance.PlayerLastCheckPoint == null) {
            GameIniciator.Instance.RespawnControllerInstance.TurnResetable(this);
            return;
        }
        if (GameIniciator.Instance.RespawnControllerInstance.PlayerLastCheckPoint == _linkedCheckpoint) {
            GameIniciator.Instance.RespawnControllerInstance.TurnNonResetable(this);
        }
    }

    //REMENDO RETIRAR APS CONVERSA SOBRE RESETABLE
    IEnumerator ActivateResetable() {
        yield return new WaitForSeconds(Time.deltaTime * 4);
        GameIniciator.Instance.RespawnControllerInstance.OnPlayerChangeCheckPoint += VerifyReset;
        GameIniciator.Instance.RespawnControllerInstance.TurnResetable(this);
    }

    //public abstract void SetMemory();

    public abstract void ResetObject();
}
