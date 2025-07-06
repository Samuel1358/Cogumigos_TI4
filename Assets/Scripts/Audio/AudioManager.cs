using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour {
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private List<SoundEffect> soundEffects = new List<SoundEffect>();
    
    [Header("Background Music Settings")]
    [SerializeField] private AudioClip tutorialMusic;
    [SerializeField] private AudioClip level1Music;
    [SerializeField] private AudioClip level2Music;
    [SerializeField] private AudioClip level3Music;
    [SerializeField] private float musicFadeInDuration = 2f;
    [SerializeField] private float musicFadeOutDuration = 1f;

    [Header("Proximity Audio Settings")]
    [SerializeField] private float proximityDistance = 20f;              // Distance to check for proximity
    [SerializeField] private bool showProximityDebug = false;           // Show debug info

    private Dictionary<string, AudioClip> soundEffectDictionary = new Dictionary<string, AudioClip>();
    private Transform playerTransform;

    // Volume settings
    private float masterVolume = 1f;
    private float sfxVolume = 1f;
    private float bgmVolume = 1f;

    // Death sound control
    private bool isDeathSoundPlaying = false;
    private const float DEATH_SOUND_COOLDOWN = 3.5f;
    
    // Background music control
    private AudioClip currentBGM;
    private Coroutine fadeCoroutine;
    private bool isMusicFading = false;

    // PlayerPrefs keys
    private const string MASTER_VOLUME_KEY = "MasterVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";
    private const string BGM_VOLUME_KEY = "BGMVolume";

    private void Awake() {
        InitializeAudioManager();
        LoadVolumeSettings();
        
        // Subscribe to scene change events
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void Start() {
        // Start music for current scene
        AudioClip currentSceneMusic = GetMusicForScene(SceneManager.GetActiveScene().name);
        if (currentSceneMusic != null) {
            currentBGM = currentSceneMusic;
            bgmSource.clip = currentBGM;
            bgmSource.Play();
            bgmSource.volume = masterVolume * bgmVolume;
            
            if (showProximityDebug) {
                Debug.Log($"AudioManager: Started with BGM for current scene: {currentBGM.name}");
            }
        }
    }

    private void InitializeAudioManager() {
        if (sfxSource == null) {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }

        if (bgmSource == null) {
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;
        }

        // Inicializa o dicionário com os efeitos sonoros
        foreach (var sound in soundEffects) {
            if (!string.IsNullOrEmpty(sound.name) && sound.clip != null) {
                soundEffectDictionary[sound.name] = sound.clip;
            }
        }
    }

    private void LoadVolumeSettings() {
        masterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, 1f);
        sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);
        bgmVolume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, 1f);

        ApplyVolumeSettings();
    }

    private void ApplyVolumeSettings() {
        sfxSource.volume = masterVolume * sfxVolume;
        
        // Only apply BGM volume if not currently fading
        if (!isMusicFading) {
            bgmSource.volume = masterVolume * bgmVolume;
        }
    }

    public void SetMasterVolume(float volume) {
        masterVolume = volume;
        PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, volume);
        PlayerPrefs.Save();
        ApplyVolumeSettings();
    }

    public void SetSFXVolume(float volume) {
        sfxVolume = volume;
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);
        PlayerPrefs.Save();
        ApplyVolumeSettings();
    }

    public void SetBGMVolume(float volume) {
        bgmVolume = volume;
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, volume);
        PlayerPrefs.Save();
        ApplyVolumeSettings();
    }

    public void PlaySFX(string soundName, float volume = 1f, float pitch = 1f) {
        if (soundEffectDictionary.TryGetValue(soundName, out AudioClip clip)) {
            sfxSource.pitch = pitch;
            sfxSource.PlayOneShot(clip, volume);
        }
        else {
            Debug.LogWarning($"Sound effect '{soundName}' not found!");
        }
    }

    // Special method for playing death sound with cooldown control
    public void PlayDeathSound() {
        if (!isDeathSoundPlaying) {
            if (soundEffectDictionary.TryGetValue("Death", out AudioClip clip)) {
                isDeathSoundPlaying = true;
                sfxSource.pitch = 1f;
                sfxSource.PlayOneShot(clip, 1f);

                // Reset the flag after cooldown
                StartCoroutine(ResetDeathSoundCooldown());
            }
            else {
                Debug.LogWarning("Death sound effect not found!");
            }
        }
    }

    private IEnumerator ResetDeathSoundCooldown() {
        yield return new WaitForSeconds(DEATH_SOUND_COOLDOWN);
        isDeathSoundPlaying = false;
    }

    /// <summary>
    /// Plays a sound effect at a specific position with 3D audio
    /// </summary>
    public void PlaySFXAtPosition(string soundName, Vector3 position, float volume = 1f, float pitch = 1f) {
        if (soundEffectDictionary.TryGetValue(soundName, out AudioClip clip)) {
            float totalVolume = volume * masterVolume * sfxVolume;
            AudioSource.PlayClipAtPoint(clip, position, totalVolume);
        }
        else {
            Debug.LogWarning($"Sound effect '{soundName}' not found!");
        }
    }

    /// <summary>
    /// Plays a sound effect only if the player is within proximity distance
    /// </summary>
    public bool PlaySFXIfPlayerNearby(string soundName, Vector3 position, float volume = 1f, float pitch = 1f) {
        if (playerTransform == null) {
            // Try to find player
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) {
                playerTransform = player.transform;
            }
            else {
                if (showProximityDebug)
                    Debug.LogWarning("AudioManager: Player not found! Cannot check proximity.");
                return false;
            }
        }

        float distance = Vector3.Distance(position, playerTransform.position);

        if (distance <= proximityDistance) {
            PlaySFXAtPosition(soundName, position, volume, pitch);

            if (showProximityDebug)
                Debug.Log($"AudioManager: Playing '{soundName}' at distance {distance:F2} (max: {proximityDistance})");

            return true;
        }
        else {
            if (showProximityDebug)
                Debug.Log($"AudioManager: Player too far for '{soundName}' - distance {distance:F2} (max: {proximityDistance})");

            return false;
        }
    }

    /// <summary>
    /// Sets the player transform for proximity calculations
    /// </summary>
    public void SetPlayerTransform(Transform player) {
        playerTransform = player;
        if (showProximityDebug)
            Debug.Log($"AudioManager: Player transform set to {player.name}");
    }

    /// <summary>
    /// Checks if a position is within proximity distance of the player
    /// </summary>
    public bool IsPlayerNearby(Vector3 position) {
        if (playerTransform == null) {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) {
                playerTransform = player.transform;
            }
            else {
                return false;
            }
        }

        float distance = Vector3.Distance(position, playerTransform.position);
        return distance <= proximityDistance;
    }

    public void PlayBGM(AudioClip bgmClip) {
        if (bgmClip != null) {
            bgmSource.clip = bgmClip;
            bgmSource.Play();
        }
    }

    public void StopBGM() {
        bgmSource.Stop();
    }

    public void StopSFX() {
        sfxSource.Stop();
    }

    public void SetSFXPitch(float pitch) {
        sfxSource.pitch = pitch;
    }
    
    // Scene change handling
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (showProximityDebug) {
            Debug.Log($"AudioManager: Scene loaded: {scene.name}");
        }
        
        // Determine which music to play based on scene name
        AudioClip musicToPlay = GetMusicForScene(scene.name);
        
        if (musicToPlay != null && musicToPlay != currentBGM) {
            PlayBGMWithFade(musicToPlay);
        }
    }
    
    private AudioClip GetMusicForScene(string sceneName) {
        sceneName = sceneName.ToLower();
        
        if (sceneName.Contains("tutorial") || sceneName.Contains("mainmenu")) {
            return tutorialMusic;
        }
        else if (sceneName.Contains("level01")) {
            return level1Music;
        }
        else if (sceneName.Contains("level02")) {
            return level2Music;
        }
        else if (sceneName.Contains("level03")) {
            return level3Music;
        }
        
        // Default to tutorial music if no match found
        return tutorialMusic;
    }
    
    /// <summary>
    /// Força a volta para a música do menu (útil quando retornar ao menu)
    /// </summary>
    public void ReturnToMenuMusic() {
        if (tutorialMusic != null && tutorialMusic != currentBGM) {
            PlayBGMWithFade(tutorialMusic);
        }
    }
    
    public void PlayBGMWithFade(AudioClip bgmClip) {
        if (bgmClip == null) return;
        
        // Stop any ongoing fade
        if (fadeCoroutine != null) {
            StopCoroutine(fadeCoroutine);
        }
        
        fadeCoroutine = StartCoroutine(FadeBGM(bgmClip));
    }
    
    private IEnumerator FadeBGM(AudioClip newBGM) {
        isMusicFading = true;
        
        // Fade out current music
        if (bgmSource.isPlaying && bgmSource.clip != null) {
            float startVolume = bgmSource.volume;
            float fadeOutTime = 0f;
            
            while (fadeOutTime < musicFadeOutDuration) {
                fadeOutTime += Time.deltaTime;
                float normalizedTime = fadeOutTime / musicFadeOutDuration;
                bgmSource.volume = Mathf.Lerp(startVolume, 0f, normalizedTime);
                yield return null;
            }
        }
        
        // Change to new music
        bgmSource.clip = newBGM;
        currentBGM = newBGM;
        bgmSource.Play();
        
        // Fade in new music
        float targetVolume = masterVolume * bgmVolume;
        float fadeInTime = 0f;
        
        while (fadeInTime < musicFadeInDuration) {
            fadeInTime += Time.deltaTime;
            float normalizedTime = fadeInTime / musicFadeInDuration;
            bgmSource.volume = Mathf.Lerp(0f, targetVolume, normalizedTime);
            yield return null;
        }
        
        // Ensure final volume is correct
        bgmSource.volume = targetVolume;
        isMusicFading = false;
        fadeCoroutine = null;
        
        if (showProximityDebug) {
            Debug.Log($"AudioManager: BGM changed to {newBGM.name}");
        }
    }

    // Getters for current volume values
    public float GetMasterVolume() => masterVolume;
    public float GetSFXVolume() => sfxVolume;
    public float GetBGMVolume() => bgmVolume;

    private void OnDrawGizmosSelected() {
        // Draw proximity range in scene view
        if (playerTransform != null) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(playerTransform.position, proximityDistance);
        }
    }
    
    private void OnDestroy() {
        // Unsubscribe from scene change events
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}