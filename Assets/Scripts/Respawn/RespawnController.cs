using System;
using System.Collections.Generic;
using UnityEngine;

public class RespawnController : MonoBehaviour {
    public Checkpoint PlayerActiveCheckPoint { get; private set; }
    public Checkpoint PlayerLastCheckPoint { get; private set; }
    public List<IResetable> ResetablesTraps { get; private set; }

    public bool isFirstActivation;

    public int CheckpointCount = 0;

    public Action OnPlayerRespawn;
    public Action<Checkpoint> OnPlayerChangeCheckPoint;

    private void Start() {
        ResetablesTraps = new List<IResetable>();
        CheckpointCount = 0;
        OnPlayerRespawn += ResetObjects;
    }

    private void ResetObjects() {
        Debug.Log($"RespawnController: ResetObjects chamado - {ResetablesTraps.Count} objetos para resetar");
        foreach (IResetable trap in ResetablesTraps) {
            Debug.Log($"RespawnController: Resetando objeto {trap.GetType().Name} - {trap}");
            trap.ResetObject();
        }
        Debug.Log("RespawnController: ResetObjects concluído");
    }

    public void SetActiveCheckPoint(Checkpoint newCheckpoint) {
        PlayerLastCheckPoint = PlayerActiveCheckPoint;
        PlayerActiveCheckPoint = newCheckpoint;
    }

    public void TurnResetable(IResetable trap) {
        if (!ResetablesTraps.Contains(trap))
        {
            ResetablesTraps.Add(trap);
        }
    }
    
    public void TurnNonResetable(IResetable trap) {
        ResetablesTraps.Remove(trap);
    }
    public void ResetController() {
        ResetablesTraps = new List<IResetable>();
    }
}
