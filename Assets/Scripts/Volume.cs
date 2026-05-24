using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioMixer mainMixer; // Drag your 'MainMixer' asset here
    public Slider masterSlider;  // Drag your UI Slider here
    void Start()
    {
        // Ensure the slider matches the current volume of the mixer
        float currentVol;
        mainMixer.GetFloat("MasterVolume", out currentVol);

        // Convert dB back to 0-1 range for the slider
        masterSlider.value = Mathf.Pow(10, currentVol / 20);
    }
    public void SetMasterVolume()
    {
        // Get the value (0 to 1) from the slider
        float volume = masterSlider.value;

        // Convert to Decibels (-80 to 0)
        // If slider is 0 (mute), set to -80dB, otherwise calculate log
        float dbValue = (volume <= 0.001f) ? -80f : Mathf.Log10(volume) * 20f;

        mainMixer.SetFloat("MasterVolume", dbValue);
    }
}