using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private VolumeType volumeType;

    private void Start()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }

        // Initialize slider with saved value
        float savedVolume = GetSavedVolume();
        slider.value = savedVolume;
        UpdateValueText(savedVolume);

        // Add listener for value changes
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private float GetSavedVolume()
    {
        switch (volumeType)
        {
            case VolumeType.Master:
                return GameIniciator.Instance.AudioManagerInstance.GetMasterVolume();
            case VolumeType.SFX:
                return GameIniciator.Instance.AudioManagerInstance.GetSFXVolume();
            case VolumeType.BGM:
                return GameIniciator.Instance.AudioManagerInstance.GetBGMVolume();
            default:
                return 1f;
        }
    }

    private void OnSliderValueChanged(float value)
    {
        UpdateValueText(value);
        
        switch (volumeType)
        {
            case VolumeType.Master:
                GameIniciator.Instance.AudioManagerInstance.SetMasterVolume(value);
                break;
            case VolumeType.SFX:
                GameIniciator.Instance.AudioManagerInstance.SetSFXVolume(value);
                break;
            case VolumeType.BGM:
                GameIniciator.Instance.AudioManagerInstance.SetBGMVolume(value);
                break;
        }
    }

    private void UpdateValueText(float value)
    {
        if (valueText != null)
        {
            valueText.text = Mathf.RoundToInt(value * 100) + "%";
        }
    }
}

public enum VolumeType
{
    Master,
    SFX,
    BGM
} 