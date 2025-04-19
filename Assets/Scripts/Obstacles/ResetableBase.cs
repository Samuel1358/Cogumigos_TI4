using UnityEngine;

public abstract class ResetableBase : MonoBehaviour, IResetable
{
    [SerializeField] private Checkpoint _linkedCheckpoint;

    protected bool NeedReset;

    private void OnEnable() {
        RespawnController.OnPlayerChangeCheckPoint += VerifyReset;
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

    public virtual void ResetObject() {
    }
}
