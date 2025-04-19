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
        OnPlayerRespawn += ResetObjects;
    }

    private void ResetObjects() {
        foreach (IResetable trap in ResetablesTraps) {
            trap.ResetObject();
        }
    }

    public void SetActiveCheckPoint(Checkpoint newCheckpoint) {
        PlayerLastCheckPoint = PlayerActiveCheckPoint;
        PlayerActiveCheckPoint = newCheckpoint;
    }

    public void TurnResetable(IResetable trap) {
        ResetablesTraps.Add(trap);
    }
    public void TurnNonResetable(IResetable trap) {
        ResetablesTraps.Remove(trap);
    }
}
