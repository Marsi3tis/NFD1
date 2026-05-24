using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for the Toggle component
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle; // Drag your Checkbox here

    private List<Resolution> filteredResolutions = new List<Resolution>();

    void Start()
    {
        // 1. Initialize List and UI
        resolutionDropdown.ClearOptions();
        filteredResolutions.Clear();

        Resolution[] allResolutions = Screen.resolutions;
        List<string> options = new List<string>();

        foreach (Resolution res in allResolutions)
        {
            if (!filteredResolutions.Exists(r => r.width == res.width && r.height == res.height))
            {
                filteredResolutions.Add(res);
                options.Add(res.width + " x " + res.height);
            }
        }

        resolutionDropdown.AddOptions(options);

        // 2. Sync Toggle state with current screen state
        fullscreenToggle.isOn = Screen.fullScreen;
    }

    // Call this method via the Button's "On Click" event
    public void ApplySettings()
    {
        // Get index from Dropdown and state from Toggle
        int index = resolutionDropdown.value;
        bool isFullscreen = fullscreenToggle.isOn;

        if (index >= 0 && index < filteredResolutions.Count)
        {
            Resolution res = filteredResolutions[index];

            // Apply both at once
            Screen.SetResolution(res.width, res.height, isFullscreen);

            Debug.Log($"Applied: {res.width}x{res.height} | Fullscreen: {isFullscreen}");
        }
    }
}