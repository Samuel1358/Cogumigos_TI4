using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Collider), typeof(Volume))]
public class SimplePostProcessingArea : MonoBehaviour
{
    [Header("Area Settings")]
    [SerializeField] private string areaName = "Post Processing Area";
    [SerializeField] private AreaType areaType = AreaType.Bright;
    [SerializeField] private float transitionDuration = 1.5f;
    
    [Header("Player Detection")]
    [SerializeField] private LayerMask playerLayer = 1 << 3; // Layer 3 = Player
    
    public enum AreaType
    {
        Bright,  // Bright and happy area
        Dark     // Dark and gloomy area
    }
    
    private Volume volume;
    private Collider areaCollider;
    private bool playerInArea = false;
    private Coroutine transitionCoroutine;
    
    // Static control to avoid multiple active areas
    private static SimplePostProcessingArea currentActiveArea;
    
    void Start()
    {
        SetupArea();
    }
    
    void SetupArea()
    {
        // Setup collider
        areaCollider = GetComponent<Collider>();
        areaCollider.isTrigger = true;
        
        // Setup volume
        volume = GetComponent<Volume>();
        volume.isGlobal = false;
        volume.weight = 0f; // Start deactivated
        volume.priority = 1; // Higher priority than global volumes
        
        // Create profile based on type
        CreateVolumeProfile();
    }
    
    void CreateVolumeProfile()
    {
        VolumeProfile profile = ScriptableObject.CreateInstance<VolumeProfile>();
        
        if (areaType == AreaType.Bright)
        {
            SetupBrightArea(profile);
        }
        else
        {
            SetupDarkArea(profile);
        }
        
        volume.profile = profile;
    }
    
    void SetupBrightArea(VolumeProfile profile)
    {
        // More noticeable bloom for warmth
        var bloom = profile.Add<Bloom>();
        bloom.active = true;
        bloom.intensity.overrideState = true;
        bloom.intensity.value = 0.6f; // More visible bloom
        bloom.threshold.overrideState = true;
        bloom.threshold.value = 0.7f; // Lower threshold for more bloom
        bloom.scatter.overrideState = true;
        bloom.scatter.value = 0.7f;
        
        // More vibrant color adjustments
        var colorAdjustments = profile.Add<ColorAdjustments>();
        colorAdjustments.active = true;
        colorAdjustments.postExposure.overrideState = true;
        colorAdjustments.postExposure.value = 0.4f; // More brightness
        colorAdjustments.saturation.overrideState = true;
        colorAdjustments.saturation.value = 25f; // More saturated
        colorAdjustments.contrast.overrideState = true;
        colorAdjustments.contrast.value = 8f; // Bit more contrast
        colorAdjustments.hueShift.overrideState = true;
        colorAdjustments.hueShift.value = 12f; // More golden shift
        
        // Stronger warm white balance
        var whiteBalance = profile.Add<WhiteBalance>();
        whiteBalance.active = true;
        whiteBalance.temperature.overrideState = true;
        whiteBalance.temperature.value = 35f; // Warmer/more golden tone
        whiteBalance.tint.overrideState = true;
        whiteBalance.tint.value = 5f;
        
        // Add subtle golden vignette
        var vignette = profile.Add<Vignette>();
        vignette.active = true;
        vignette.intensity.overrideState = true;
        vignette.intensity.value = 0.15f; // Very subtle
        vignette.smoothness.overrideState = true;
        vignette.smoothness.value = 0.6f;
        vignette.color.overrideState = true;
        vignette.color.value = new Color(1f, 0.95f, 0.7f, 1f); // Subtle golden tone
    }
    
    void SetupDarkArea(VolumeProfile profile)
    {
        // Subtle dark and moody color adjustments
        var colorAdjustments = profile.Add<ColorAdjustments>();
        colorAdjustments.active = true;
        colorAdjustments.postExposure.overrideState = true;
        colorAdjustments.postExposure.value = -0.3f; // Slightly darker, still visible
        colorAdjustments.saturation.overrideState = true;
        colorAdjustments.saturation.value = -20f; // Moderately desaturated
        colorAdjustments.contrast.overrideState = true;
        colorAdjustments.contrast.value = 8f; // Subtle contrast increase
        colorAdjustments.hueShift.overrideState = true;
        colorAdjustments.hueShift.value = -10f; // Slight cold shift
        
        // Subtle dark vignette
        var vignette = profile.Add<Vignette>();
        vignette.active = true;
        vignette.intensity.overrideState = true;
        vignette.intensity.value = 0.2f; // Much more subtle
        vignette.smoothness.overrideState = true;
        vignette.smoothness.value = 0.5f;
        vignette.color.overrideState = true;
        vignette.color.value = new Color(0.3f, 0.3f, 0.4f, 1f); // Subtle blue-gray tone
        
        // Cold white balance (main effect)
        var whiteBalance = profile.Add<WhiteBalance>();
        whiteBalance.active = true;
        whiteBalance.temperature.overrideState = true;
        whiteBalance.temperature.value = -20f; // Cool/blue tone
        whiteBalance.tint.overrideState = true;
        whiteBalance.tint.value = -3f;
        
        // Very subtle shadows adjustment
        var shadowsMidtonesHighlights = profile.Add<ShadowsMidtonesHighlights>();
        shadowsMidtonesHighlights.active = true;
        shadowsMidtonesHighlights.shadows.overrideState = true;
        shadowsMidtonesHighlights.shadows.value = new Vector4(0.8f, 0.85f, 1.1f, -0.1f); // Subtle blue shadows
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (IsPlayerLayer(other.gameObject.layer))
        {
            EnterArea();
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (IsPlayerLayer(other.gameObject.layer))
        {
            ExitArea();
        }
    }
    
    public void EnterArea()
    {
        if (playerInArea) return;
        
        playerInArea = true;
        
        // Deactivate previous area if any
        if (currentActiveArea != null && currentActiveArea != this)
        {
            currentActiveArea.ExitArea();
        }
        
        currentActiveArea = this;
        
        // Stop previous transition if running
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }
        
        // Start transition to activate
        transitionCoroutine = StartCoroutine(TransitionVolume(true));
    }
    
    public void ExitArea()
    {
        if (!playerInArea) return;
        
        playerInArea = false;
        
        if (currentActiveArea == this)
        {
            currentActiveArea = null;
        }
        
        // Stop previous transition if running
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }
        
        // Start transition to deactivate
        transitionCoroutine = StartCoroutine(TransitionVolume(false));
    }
    
    System.Collections.IEnumerator TransitionVolume(bool activate)
    {
        float startWeight = volume.weight;
        float targetWeight = activate ? 1f : 0f;
        float elapsedTime = 0f;
        
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / transitionDuration;
            
            // Smooth curve for transition
            float smoothTime = Mathf.SmoothStep(0f, 1f, normalizedTime);
            volume.weight = Mathf.Lerp(startWeight, targetWeight, smoothTime);
            
            yield return null;
        }
        
        volume.weight = targetWeight;
        transitionCoroutine = null;
    }
    
    private bool IsPlayerLayer(int layer)
    {
        return (playerLayer.value & (1 << layer)) != 0;
    }
    
    // Gizmos for editor visualization
    void OnDrawGizmos()
    {
        Collider col = GetComponent<Collider>();
        if (col == null) return;
        
        // Color based on type
        Color gizmoColor = areaType == AreaType.Bright ? Color.yellow : Color.red;
        gizmoColor.a = 0.3f;
        
        Gizmos.color = gizmoColor;
        
        // Draw bounds
        if (col is BoxCollider box)
        {
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.DrawCube(box.center, box.size);
        }
        else if (col is SphereCollider sphere)
        {
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.DrawSphere(sphere.center, sphere.radius);
        }
        
        Gizmos.matrix = Matrix4x4.identity;
        
        // Label in editor
        #if UNITY_EDITOR
        if (Application.isEditor)
        {
            UnityEditor.Handles.Label(transform.position + Vector3.up * 2f, $"{areaName}\n({areaType})");
        }
        #endif
    }
} 