using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private List<SoundEffect> soundEffects = new List<SoundEffect>();

    private Dictionary<string, AudioClip> soundEffectDictionary = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioManager();
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

        // Inicializa o dicionário com os efeitos sonoros
        foreach (var sound in soundEffects)
        {
            if (!string.IsNullOrEmpty(sound.name) && sound.clip != null)
            {
                soundEffectDictionary[sound.name] = sound.clip;
            }
        }
    }

    public void PlaySFX(string soundName, float volume = 1f, float pitch = 1f)
    {
        if (soundEffectDictionary.TryGetValue(soundName, out AudioClip clip))
        {
            sfxSource.pitch = pitch;
            sfxSource.PlayOneShot(clip, volume);
        }
        else
        {
            Debug.LogWarning($"Sound effect '{soundName}' not found!");
        }
    }

    public void PlaySFXAtPosition(string soundName, Vector3 position, float volume = 1f, float pitch = 1f)
    {
        if (soundEffectDictionary.TryGetValue(soundName, out AudioClip clip))
        {
            AudioSource.PlayClipAtPoint(clip, position, volume);
        }
        else
        {
            Debug.LogWarning($"Sound effect '{soundName}' not found!");
        }
    }

    public void StopSFX()
    {
        sfxSource.Stop();
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    public void SetSFXPitch(float pitch)
    {
        sfxSource.pitch = pitch;
    }
} 