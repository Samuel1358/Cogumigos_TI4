using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LillypadSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject lillypadPrefab;            // Lillypad prefab to spawn
    [SerializeField] private float spawnInterval = 2f;            // Time between spawns
    [SerializeField] private bool autoStart = true;               // Start spawning automatically
    
    [Header("Waypoints")]
    [SerializeField] private Transform[] spawnPoints = new Transform[4];     // 4 spawn points
    [SerializeField] private Transform[] middlePoints = new Transform[4];    // 4 middle points
    [SerializeField] private Transform[] finalPoints = new Transform[4];     // 4 final points
    
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 3f;            // Speed moving between waypoints
    
    [Header("Sink Settings")]
    [SerializeField] private float sinkSpeed = 2f;                // Speed of sinking
    [SerializeField] private float sinkDepth = 3f;                // How deep to sink before disappearing
    [SerializeField] private float disappearDelay = 1f;           // Time before starting to sink
    
    private bool isSpawning = false;
    private List<Lillypad> activeLillypads = new List<Lillypad>();
    private int lastUsedSpawnPointIndex = -1;                     // Track last used spawn point to avoid consecutive spawns

    private void Start()
    {
        if (autoStart)
        {
            StartSpawning();
        }
    }

    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnRoutine());
        }
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }

    private IEnumerator SpawnRoutine()
    {
        while (isSpawning)
        {
            SpawnLillypad();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnLillypad()
    {
        if (spawnPoints.Length != 4 || middlePoints.Length != 4 || finalPoints.Length != 4 || lillypadPrefab == null)
        {
            return;
        }

        int selectedSpawnIndex = ChooseSpawnPoint();
        Transform spawnPoint = spawnPoints[selectedSpawnIndex];
        lastUsedSpawnPointIndex = selectedSpawnIndex;

        List<Transform> path = GeneratePath(selectedSpawnIndex);

        GameObject newLillypad = Instantiate(lillypadPrefab, spawnPoint.position, spawnPoint.rotation);
        
        Lillypad lillypadScript = newLillypad.GetComponent<Lillypad>();
        if (lillypadScript == null)
        {
            Destroy(newLillypad);
            return;
        }

        lillypadScript.SetWaypointControl(true, path, movementSpeed, sinkSpeed, sinkDepth, disappearDelay);
        lillypadScript.OnLillypadDestroyed += RemoveLillypadFromList;
        
        activeLillypads.Add(lillypadScript);
    }

    private List<Transform> GeneratePath(int spawnIndex)
    {
        List<Transform> path = new List<Transform>();
        
        path.Add(spawnPoints[spawnIndex]);
        
        List<int> availableMiddleIndices = GetAdjacentMiddlePoints(spawnIndex);
        int selectedMiddleIndex = availableMiddleIndices[Random.Range(0, availableMiddleIndices.Count)];
        path.Add(middlePoints[selectedMiddleIndex]);
        
        List<int> availableFinalIndices = GetAdjacentFinalPoints(selectedMiddleIndex);
        int selectedFinalIndex = availableFinalIndices[Random.Range(0, availableFinalIndices.Count)];
        path.Add(finalPoints[selectedFinalIndex]);
        
        return path;
    }

    private List<int> GetAdjacentMiddlePoints(int spawnIndex)
    {
        List<int> adjacent = new List<int>();
        adjacent.Add(spawnIndex);
        adjacent.Add((spawnIndex + 1) % 4);
        return adjacent;
    }

    private List<int> GetAdjacentFinalPoints(int middleIndex)
    {
        List<int> adjacent = new List<int>();
        
        for (int i = -1; i <= 1; i++)
        {
            int finalIndex = middleIndex + i;
            if (finalIndex >= 0 && finalIndex < 4)
            {
                adjacent.Add(finalIndex);
            }
        }
        
        return adjacent;
    }

    private int ChooseSpawnPoint()
    {
        if (lastUsedSpawnPointIndex == -1)
        {
            return Random.Range(0, 4);
        }

        List<int> availableIndices = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            if (i != lastUsedSpawnPointIndex)
            {
                availableIndices.Add(i);
            }
        }

        return availableIndices[Random.Range(0, availableIndices.Count)];
    }

    private void RemoveLillypadFromList(Lillypad lillypad)
    {
        activeLillypads.Remove(lillypad);
    }

    private void OnDrawGizmosSelected()
    {
        if (spawnPoints != null)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (spawnPoints[i] != null)
                {
                    Gizmos.DrawWireSphere(spawnPoints[i].position, 0.5f);
                    
                    List<int> adjacentMiddles = GetAdjacentMiddlePoints(i);
                    foreach (int middleIndex in adjacentMiddles)
                    {
                        if (middlePoints[middleIndex] != null)
                        {
                            Gizmos.color = Color.yellow;
                            Gizmos.DrawLine(spawnPoints[i].position, middlePoints[middleIndex].position);
                            Gizmos.color = Color.green;
                        }
                    }
                }
            }
        }

        if (middlePoints != null)
        {
            Gizmos.color = Color.blue;
            for (int i = 0; i < middlePoints.Length; i++)
            {
                if (middlePoints[i] != null)
                {
                    Gizmos.DrawWireSphere(middlePoints[i].position, 0.5f);
                    
                    List<int> adjacentFinals = GetAdjacentFinalPoints(i);
                    foreach (int finalIndex in adjacentFinals)
                    {
                        if (finalPoints[finalIndex] != null)
                        {
                            Gizmos.color = Color.cyan;
                            Gizmos.DrawLine(middlePoints[i].position, finalPoints[finalIndex].position);
                            Gizmos.color = Color.blue;
                        }
                    }
                }
            }
        }

        if (finalPoints != null)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < finalPoints.Length; i++)
            {
                if (finalPoints[i] != null)
                {
                    Gizmos.DrawWireSphere(finalPoints[i].position, 0.5f);
                }
            }
        }
    }
}
