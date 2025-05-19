using UnityEngine;

public class PlatformChain : MonoBehaviour
{
    [Header("Movement Settings")]
    public float platformSpeed = 2f;
    public bool moveRight = true;

    [Header("Reset Settings")]
    public Transform resetPoint;
    public float resetXPosition = 20f;

    private Platform[] platforms;

    void Start()
    {
        // Get all platform components from children
        platforms = GetComponentsInChildren<Platform>();
        
        // Initialize each platform
        foreach (Platform platform in platforms)
        {
            platform.Initialize(platformSpeed, moveRight, resetPoint.position, resetXPosition);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
