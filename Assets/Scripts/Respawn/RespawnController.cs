using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RespawnController : MonoBehaviour {
    public static RespawnController Instance { get; private set; }
    public Checkpoint PlayerActiveCheckPoint { get; private set; }
    public Checkpoint PlayerLastCheckPoint { get; private set; }
    public List<IResetable> ResetablesTraps { get; private set; }

    public static bool isFirstActivation;

    public static int CheckpointCount = 0;

    public static Action OnPlayerRespawn;
    public static Action<Checkpoint> OnPlayerChangeCheckPoint;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
        ResetablesTraps = new List<IResetable>();
        CheckpointCount = 0;
        OnPlayerRespawn += ResetTraps;
    }

    private void ResetTraps() {
        foreach (IResetable trap in ResetablesTraps) {
            trap.ResetTrap();
        }
    }

    public void SetActiveCheckPoint(Checkpoint newCheckpoint) {
        PlayerLastCheckPoint = PlayerActiveCheckPoint;
        PlayerActiveCheckPoint = newCheckpoint;
    }

    public void TurnTrapResetable(IResetable trap) {
        ResetablesTraps.Add(trap);
    }
    public void TurnTrapNonResetable(IResetable trap) {
        ResetablesTraps.Remove(trap);
    }
}
