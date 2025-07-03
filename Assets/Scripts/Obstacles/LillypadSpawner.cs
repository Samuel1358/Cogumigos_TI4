using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class WaypointPhase
{
    [Header("Waypoint Phase")]
    public string phaseName = "Phase";
    public Transform[] waypoints = new Transform[0];
    [Range(1, 3)]
    public int maxAdjacentDistance = 1; // Maximum distance for considering waypoints as adjacent
    
    [Header("Validation")]
    [SerializeField] private bool showValidationWarnings = true;
    
    public bool IsValid()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            if (showValidationWarnings)
                Debug.LogWarning($"WaypointPhase '{phaseName}': No waypoints assigned!");
            return false;
        }
        
        // Check for null waypoints
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] == null)
            {
                if (showValidationWarnings)
                    Debug.LogWarning($"WaypointPhase '{phaseName}': Waypoint at index {i} is null!");
                return false;
            }
        }
        
        return true;
    }
}

public class LillypadSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject lillypadPrefab;            // Lillypad prefab to spawn
    [SerializeField] private float spawnInterval = 2f;            // Time between spawns
    [SerializeField] private bool autoStart = true;               // Start spawning automatically
    
    [Header("Waypoint Phases")]
    [SerializeField] private WaypointPhase[] waypointPhases = new WaypointPhase[3]; // Dynamic phases (start, middle, end)
    
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 3f;            // Speed moving between waypoints
    
    [Header("Sink Settings")]
    [SerializeField] private float sinkSpeed = 2f;                // Speed of sinking
    [SerializeField] private float sinkDepth = 3f;                // How deep to sink before disappearing
    [SerializeField] private float disappearDelay = 1f;           // Time before starting to sink
    
    [Header("Debug")]
    [SerializeField] private bool showDebugInfo = false;
    
    private bool isSpawning = false;
    private List<Lillypad> activeLillypads = new List<Lillypad>();
    private int lastUsedSpawnPointIndex = -1;                     // Track last used spawn point to avoid consecutive spawns
    
    // Track last used waypoints for each phase to avoid repetition
    private int[] lastUsedWaypointIndices;                        // Array to track last used waypoint for each phase

    private ObjectPool _objectPool;

    private void Awake()
    {
        _objectPool = ObjectPool.CreateObjecPool(name + "_ObjectPool", new Vector3(1000, 1000, 1000));
        _objectPool.SetInstanceObject(lillypadPrefab);
    }

    private void Start()
    {
        ValidateConfiguration();
        
        // Initialize the tracking array for waypoint indices
        if (waypointPhases != null && waypointPhases.Length > 0)
        {
            lastUsedWaypointIndices = new int[waypointPhases.Length];
            for (int i = 0; i < lastUsedWaypointIndices.Length; i++)
            {
                lastUsedWaypointIndices[i] = -1; // -1 means no waypoint used yet
            }
        }
        
        if (autoStart)
        {
            StartSpawning();
        }
    }

    /// <summary>
    /// Validates the configuration of waypoint phases
    /// </summary>
    private void ValidateConfiguration()
    {
        if (waypointPhases == null || waypointPhases.Length == 0)
        {
            Debug.LogError("LillypadSpawner: No waypoint phases configured!");
            return;
        }

        bool hasValidPhase = false;
        for (int i = 0; i < waypointPhases.Length; i++)
        {
            if (waypointPhases[i] != null && waypointPhases[i].IsValid())
            {
                hasValidPhase = true;
                if (showDebugInfo)
                    Debug.Log($"LillypadSpawner: Phase {i} ({waypointPhases[i].phaseName}) is valid with {waypointPhases[i].waypoints.Length} waypoints");
            }
        }

        if (!hasValidPhase)
        {
            Debug.LogError("LillypadSpawner: No valid waypoint phases found!");
        }
    }

    /// <summary>
    /// Adds a new waypoint phase to the system
    /// </summary>
    public void AddWaypointPhase(string phaseName, Transform[] waypoints, int maxAdjacentDistance = 1)
    {
        List<WaypointPhase> phasesList = new List<WaypointPhase>(waypointPhases);
        
        WaypointPhase newPhase = new WaypointPhase
        {
            phaseName = phaseName,
            waypoints = waypoints,
            maxAdjacentDistance = maxAdjacentDistance
        };
        
        phasesList.Add(newPhase);
        waypointPhases = phasesList.ToArray();
    }

    /// <summary>
    /// Removes a waypoint phase by index
    /// </summary>
    public void RemoveWaypointPhase(int index)
    {
        if (index >= 0 && index < waypointPhases.Length)
        {
            List<WaypointPhase> phasesList = new List<WaypointPhase>(waypointPhases);
            phasesList.RemoveAt(index);
            waypointPhases = phasesList.ToArray();
        }
    }

    /// <summary>
    /// Gets the number of waypoints in a specific phase
    /// </summary>
    public int GetWaypointCount(int phaseIndex)
    {
        if (phaseIndex >= 0 && phaseIndex < waypointPhases.Length && waypointPhases[phaseIndex] != null)
        {
            return waypointPhases[phaseIndex].waypoints?.Length ?? 0;
        }
        return 0;
    }

    /// <summary>
    /// Gets the total number of phases
    /// </summary>
    public int GetPhaseCount()
    {
        return waypointPhases?.Length ?? 0;
    }

    /// <summary>
    /// Gets a specific waypoint phase
    /// </summary>
    public WaypointPhase GetPhase(int phaseIndex)
    {
        if (phaseIndex >= 0 && phaseIndex < waypointPhases.Length)
        {
            return waypointPhases[phaseIndex];
        }
        return null;
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
        // Validate that we have at least one phase with waypoints
        if (waypointPhases == null || waypointPhases.Length == 0 || lillypadPrefab == null)
        {
            Debug.LogWarning("LillypadSpawner: No waypoint phases configured or missing lillypad prefab!");
            return;
        }

        // Check if first phase has waypoints
        if (waypointPhases[0].waypoints == null || waypointPhases[0].waypoints.Length == 0)
        {
            Debug.LogWarning("LillypadSpawner: First phase has no waypoints!");
            return;
        }

        int selectedSpawnIndex = ChooseSpawnPoint();
        Transform spawnPoint = waypointPhases[0].waypoints[selectedSpawnIndex];
        lastUsedSpawnPointIndex = selectedSpawnIndex;

        List<Transform> path = GeneratePath(selectedSpawnIndex);

        GameObject newLillypad = _objectPool.InstantiateObject(spawnPoint.position, spawnPoint.rotation, _objectPool.transform);//Instantiate(lillypadPrefab, spawnPoint.position, spawnPoint.rotation);
        
        Lillypad lillypadScript = newLillypad.GetComponent<Lillypad>();
        if (lillypadScript == null)
        {
            Destroy(newLillypad);
            return;
        }
        lillypadScript.Initialize(_objectPool);

        lillypadScript.SetWaypointControl(true, path, movementSpeed, sinkSpeed, sinkDepth, disappearDelay);
        lillypadScript.OnLillypadDestroyed += RemoveLillypadFromList;
        
        activeLillypads.Add(lillypadScript);
        
        if (showDebugInfo)
        {
            Debug.Log($"LillypadSpawner: Spawned lillypad with path of {path.Count} waypoints");
        }
    }

    private List<Transform> GeneratePath(int startIndex)
    {
        List<Transform> path = new List<Transform>();
        
        // Add starting waypoint
        path.Add(waypointPhases[0].waypoints[startIndex]);
        
        int currentIndex = startIndex;
        
        // Generate path through all phases
        for (int phaseIndex = 1; phaseIndex < waypointPhases.Length; phaseIndex++)
        {
            if (waypointPhases[phaseIndex].waypoints == null || waypointPhases[phaseIndex].waypoints.Length == 0)
                continue;
                
            List<int> availableIndices = GetAdjacentWaypoints(phaseIndex - 1, currentIndex, phaseIndex);
            
            if (availableIndices.Count > 0)
            {
                // Filter out the last used waypoint for this phase to avoid repetition
                List<int> filteredIndices = FilterOutLastUsedWaypoint(availableIndices, phaseIndex);
                
                int selectedIndex;
                if (filteredIndices.Count > 0)
                {
                    selectedIndex = filteredIndices[Random.Range(0, filteredIndices.Count)];
                }
                else
                {
                    // If all adjacent waypoints were used last time, use any available adjacent waypoint
                    selectedIndex = availableIndices[Random.Range(0, availableIndices.Count)];
                }
                
                path.Add(waypointPhases[phaseIndex].waypoints[selectedIndex]);
                currentIndex = selectedIndex;
                
                // Update the last used waypoint for this phase
                UpdateLastUsedWaypoint(phaseIndex, selectedIndex);
            }
            else
            {
                // If no adjacent waypoints found, use the same index if available
                if (currentIndex < waypointPhases[phaseIndex].waypoints.Length)
                {
                    path.Add(waypointPhases[phaseIndex].waypoints[currentIndex]);
                    UpdateLastUsedWaypoint(phaseIndex, currentIndex);
                }
                else
                {
                    // Fallback to first available waypoint
                    path.Add(waypointPhases[phaseIndex].waypoints[0]);
                    currentIndex = 0;
                    UpdateLastUsedWaypoint(phaseIndex, 0);
                }
            }
        }
        
        return path;
    }

    private List<int> GetAdjacentWaypoints(int fromPhaseIndex, int fromWaypointIndex, int toPhaseIndex)
    {
        List<int> adjacent = new List<int>();
        
        if (fromPhaseIndex < 0 || fromPhaseIndex >= waypointPhases.Length ||
            toPhaseIndex < 0 || toPhaseIndex >= waypointPhases.Length)
            return adjacent;
            
        WaypointPhase fromPhase = waypointPhases[fromPhaseIndex];
        WaypointPhase toPhase = waypointPhases[toPhaseIndex];
        
        if (fromPhase.waypoints == null || toPhase.waypoints == null)
            return adjacent;
            
        int maxDistance = toPhase.maxAdjacentDistance;
        
        // Calculate the range of adjacent waypoints based on the current waypoint index
        for (int i = -maxDistance; i <= maxDistance; i++)
        {
            int targetIndex = fromWaypointIndex + i;
            
            // Only consider waypoints that are within bounds (no circular wrapping)
            if (targetIndex >= 0 && targetIndex < toPhase.waypoints.Length)
            {
                adjacent.Add(targetIndex);
            }
        }
        
        return adjacent;
    }

    /// <summary>
    /// Filters out the last used waypoint for a specific phase to avoid repetition
    /// </summary>
    private List<int> FilterOutLastUsedWaypoint(List<int> availableIndices, int phaseIndex)
    {
        List<int> filteredIndices = new List<int>(availableIndices);
        
        if (lastUsedWaypointIndices != null && phaseIndex < lastUsedWaypointIndices.Length)
        {
            int lastUsedIndex = lastUsedWaypointIndices[phaseIndex];
            if (lastUsedIndex >= 0 && filteredIndices.Contains(lastUsedIndex))
            {
                filteredIndices.Remove(lastUsedIndex);
                
                if (showDebugInfo)
                {
                    Debug.Log($"LillypadSpawner: Filtered out last used waypoint {lastUsedIndex} from phase {phaseIndex}");
                }
            }
        }
        
        return filteredIndices;
    }
    
    /// <summary>
    /// Updates the last used waypoint for a specific phase
    /// </summary>
    private void UpdateLastUsedWaypoint(int phaseIndex, int waypointIndex)
    {
        if (lastUsedWaypointIndices != null && phaseIndex < lastUsedWaypointIndices.Length)
        {
            lastUsedWaypointIndices[phaseIndex] = waypointIndex;
            
            if (showDebugInfo)
            {
                Debug.Log($"LillypadSpawner: Updated last used waypoint for phase {phaseIndex} to index {waypointIndex}");
            }
        }
    }

    /// <summary>
    /// Resets the tracking of last used waypoints for all phases
    /// </summary>
    public void ResetWaypointTracking()
    {
        if (lastUsedWaypointIndices != null)
        {
            for (int i = 0; i < lastUsedWaypointIndices.Length; i++)
            {
                lastUsedWaypointIndices[i] = -1;
            }
            
            if (showDebugInfo)
            {
                Debug.Log("LillypadSpawner: Reset waypoint tracking for all phases");
            }
        }
    }
    
    /// <summary>
    /// Gets the last used waypoint index for a specific phase
    /// </summary>
    public int GetLastUsedWaypointIndex(int phaseIndex)
    {
        if (lastUsedWaypointIndices != null && phaseIndex >= 0 && phaseIndex < lastUsedWaypointIndices.Length)
        {
            return lastUsedWaypointIndices[phaseIndex];
        }
        return -1;
    }

    private int ChooseSpawnPoint()
    {
        if (waypointPhases[0].waypoints == null || waypointPhases[0].waypoints.Length == 0)
            return 0;
            
        if (lastUsedSpawnPointIndex == -1)
        {
            return Random.Range(0, waypointPhases[0].waypoints.Length);
        }

        List<int> availableIndices = new List<int>();
        for (int i = 0; i < waypointPhases[0].waypoints.Length; i++)
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

    private void OnDrawGizmos()
    {
        DrawWaypointGizmos();
    }

    private void OnDrawGizmosSelected()
    {
        DrawWaypointGizmos();
    }

    private void DrawWaypointGizmos()
    {
        if (waypointPhases == null) return;
        
        // Check if any child waypoint is selected
        bool childSelected = IsAnyChildWaypointSelected();
        
        for (int phaseIndex = 0; phaseIndex < waypointPhases.Length; phaseIndex++)
        {
            WaypointPhase phase = waypointPhases[phaseIndex];
            if (phase.waypoints == null) continue;
            
            // Set color based on phase
            Color phaseColor = GetPhaseColor(phaseIndex);
            
            // Draw waypoints for this phase
            for (int i = 0; i < phase.waypoints.Length; i++)
            {
                if (phase.waypoints[i] != null)
                {
                    // Make gizmos more visible when child is selected
                    float gizmoSize = childSelected ? 0.8f : 0.5f;
                    Color gizmoColor = phaseColor;
                    
                    // Make gizmos brighter when child is selected
                    if (childSelected)
                    {
                        gizmoColor = new Color(phaseColor.r + 0.3f, phaseColor.g + 0.3f, phaseColor.b + 0.3f, 1f);
                    }
                    
                    Gizmos.color = gizmoColor;
                    Gizmos.DrawWireSphere(phase.waypoints[i].position, gizmoSize);
                    
                    // Draw connections to next phase if available
                    if (phaseIndex < waypointPhases.Length - 1)
                    {
                        DrawConnectionsToNextPhase(phaseIndex, i, childSelected);
                    }
                }
            }
        }
    }
    
    private bool IsAnyChildWaypointSelected()
    {
        if (waypointPhases == null) return false;
        
        for (int phaseIndex = 0; phaseIndex < waypointPhases.Length; phaseIndex++)
        {
            WaypointPhase phase = waypointPhases[phaseIndex];
            if (phase.waypoints == null) continue;
            
            for (int i = 0; i < phase.waypoints.Length; i++)
            {
                if (phase.waypoints[i] != null)
                {
                    // Check if this waypoint is a child of this transform
                    if (phase.waypoints[i].IsChildOf(transform))
                    {
                        // Check if this waypoint or any of its children are selected
                        if (IsTransformOrChildrenSelected(phase.waypoints[i]))
                        {
                            return true;
                        }
                    }
                }
            }
        }
        
        return false;
    }
    
    private bool IsTransformOrChildrenSelected(Transform targetTransform)
    {
        #if UNITY_EDITOR
        // Check if the target transform itself is selected
        if (UnityEditor.Selection.Contains(targetTransform.gameObject))
        {
            return true;
        }
        
        // Check if any child is selected
        for (int i = 0; i < targetTransform.childCount; i++)
        {
            if (IsTransformOrChildrenSelected(targetTransform.GetChild(i)))
            {
                return true;
            }
        }
        #endif
        
        return false;
    }
    
    private Color GetPhaseColor(int phaseIndex)
    {
        switch (phaseIndex)
        {
            case 0: return Color.green;   // Start
            case 1: return Color.blue;    // Middle
            case 2: return Color.red;     // End
            default: return Color.white;
        }
    }
    
    private void DrawConnectionsToNextPhase(int fromPhaseIndex, int fromWaypointIndex, bool childSelected = false)
    {
        WaypointPhase fromPhase = waypointPhases[fromPhaseIndex];
        WaypointPhase toPhase = waypointPhases[fromPhaseIndex + 1];
        
        if (fromPhase.waypoints == null || toPhase.waypoints == null) return;
        
        List<int> adjacentIndices = GetAdjacentWaypoints(fromPhaseIndex, fromWaypointIndex, fromPhaseIndex + 1);
        
        foreach (int targetIndex in adjacentIndices)
        {
            if (targetIndex < toPhase.waypoints.Length && toPhase.waypoints[targetIndex] != null)
            {
                Color connectionColor = childSelected ? Color.yellow : new Color(1f, 1f, 0f, 0.5f);
                Gizmos.color = connectionColor;
                Gizmos.DrawLine(fromPhase.waypoints[fromWaypointIndex].position, 
                               toPhase.waypoints[targetIndex].position);
            }
        }
    }
}
