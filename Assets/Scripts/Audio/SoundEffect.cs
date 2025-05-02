using UnityEngine;

[System.Serializable]
public class SoundEffect
{
    [Tooltip("Nome único do efeito sonoro. Use este nome para tocar o som no código.")]
    public string name;

    [Tooltip("O clip de áudio que será tocado.")]
    public AudioClip clip;

    [Tooltip("Volume padrão do efeito sonoro (0 a 1).")]
    [Range(0f, 1f)]
    public float defaultVolume = 1f;

    [Tooltip("Pitch padrão do efeito sonoro.")]
    [Range(0.1f, 3f)]
    public float defaultPitch = 1f;
} 