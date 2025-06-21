using System.Collections;
using UnityEngine;

public abstract class ResetableBase : MonoBehaviour, IResetable
{
    [SerializeField] private Checkpoint _linkedCheckpoint;

    protected bool NeedReset;

    protected virtual void OnEnable() 
    {
        StartCoroutine(ActivateResetable());
    }

    private void OnDisable() {
        RespawnController.OnPlayerChangeCheckPoint -= VerifyReset;
        RespawnController.Instance.TurnNonResetable(this);
    }

    private void VerifyReset(Checkpoint checkpoint) {
        if (RespawnController.Instance.PlayerLastCheckPoint == null) {
            RespawnController.Instance.TurnResetable(this);
            return;
        }
        if (RespawnController.Instance.PlayerLastCheckPoint == _linkedCheckpoint) {
            RespawnController.Instance.TurnNonResetable(this);
        }
    }

    //REMENDO RETIRAR AP�S CONVERSA SOBRE RESETABLE
    IEnumerator ActivateResetable() {
        yield return new WaitForSeconds(Time.deltaTime * 4);
        RespawnController.OnPlayerChangeCheckPoint += VerifyReset;
        RespawnController.Instance.TurnResetable(this);
    }

    //public abstract void SetMemory();

    public abstract void ResetObject();
}
