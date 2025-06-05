using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private List<SoundEffect> soundEffects = new List<SoundEffect>();

    private Dictionary<string, AudioClip> soundEffectDictionary = new Dictionary<string, AudioClip>();

    // Volume settings
    private float masterVolume = 1f;
    private float sfxVolume = 1f;
    private float bgmVolume = 1f;

    // Death sound control
    private bool isDeathSoundPlaying = false;
    private const float DEATH_SOUND_COOLDOWN = 3.5f;

    // PlayerPrefs keys
    private const string MASTER_VOLUME_KEY = "MasterVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";
    private const string BGM_VOLUME_KEY = "BGMVolume";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioManager();
            LoadVolumeSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAudioManager()
    {
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }

        if (bgmSource == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;
        }

        // Inicializa o dicionário com os efeitos sonoros
        foreach (var sound in soundEffects)
        {
            if (!string.IsNullOrEmpty(sound.name) && sound.clip != null)
            {
                soundEffectDictionary[sound.name] = sound.clip;
            }
        }
    }

    private void LoadVolumeSettings()
    {
        masterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, 1f);
        sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);
        bgmVolume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, 1f);
        
        ApplyVolumeSettings();
    }

    private void ApplyVolumeSettings()
    {
        sfxSource.volume = masterVolume * sfxVolume;
        bgmSource.volume = masterVolume * bgmVolume;
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, volume);
        PlayerPrefs.Save();
        ApplyVolumeSettings();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);
        PlayerPrefs.Save();
        ApplyVolumeSettings();
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, volume);
        PlayerPrefs.Save();
        ApplyVolumeSettings();
    }

    public void PlaySFX(string soundName, float volume = 1f, float pitch = 1f)
    {
        if (soundEffectDictionary.TryGetValue(soundName, out AudioClip clip))
        {
            sfxSource.pitch = pitch;
            sfxSource.PlayOneShot(clip, 1f);
        }
        else
        {
            Debug.LogWarning($"Sound effect '{soundName}' not found!");
        }
    }

    // Special method for playing death sound with cooldown control
    public void PlayDeathSound()
    {
        if (!isDeathSoundPlaying)
        {
            if (soundEffectDictionary.TryGetValue("Death", out AudioClip clip))
            {
                isDeathSoundPlaying = true;
                sfxSource.pitch = 1f;
                sfxSource.PlayOneShot(clip, 1f);
                
                // Reset the flag after cooldown
                StartCoroutine(ResetDeathSoundCooldown());
            }
            else
            {
                Debug.LogWarning("Death sound effect not found!");
            }
        }
    }

    private IEnumerator ResetDeathSoundCooldown()
    {
        yield return new WaitForSeconds(DEATH_SOUND_COOLDOWN);
        isDeathSoundPlaying = false;
    }

    public void PlaySFXAtPosition(string soundName, Vector3 position, float volume = 1f, float pitch = 1f)
    {
        if (soundEffectDictionary.TryGetValue(soundName, out AudioClip clip))
        {
            float totalVolume = masterVolume * sfxVolume;
            AudioSource.PlayClipAtPoint(clip, position, totalVolume);
        }
        else
        {
            Debug.LogWarning($"Sound effect '{soundName}' not found!");
        }
    }

    public void PlayBGM(AudioClip bgmClip)
    {
        if (bgmClip != null)
        {
            bgmSource.clip = bgmClip;
            bgmSource.Play();
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void StopSFX()
    {
        sfxSource.Stop();
    }

    public void SetSFXPitch(float pitch)
    {
        sfxSource.pitch = pitch;
    }

    // Getters for current volume values
    public float GetMasterVolume() => masterVolume;
    public float GetSFXVolume() => sfxVolume;
    public float GetBGMVolume() => bgmVolume;
} 