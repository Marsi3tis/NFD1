using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResolutionManager : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    private List<Resolution> filteredResolutions = new List<Resolution>();

    void Start()
    {
        resolutionDropdown.ClearOptions();
        filteredResolutions.Clear();

        Resolution[] allResolutions = Screen.resolutions;
        List<string> options = new List<string>();

        // We process everything in ONE loop to ensure the index stays perfectly aligned
        foreach (Resolution res in allResolutions)
        {
            // Check if this resolution is already in our list (to prevent duplicates)
            bool isDuplicate = false;
            foreach (Resolution addedRes in filteredResolutions)
            {
                if (addedRes.width == res.width && addedRes.height == res.height)
                {
                    isDuplicate = true;
                    break;
                }
            }

            if (!isDuplicate)
            {
                filteredResolutions.Add(res);
                options.Add(res.width + " x " + res.height);
            }
        }

        // Add the combined list to the UI
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();
    }

    public void ApplyResolution(int index)
    {
        // This index comes DIRECTLY from the dropdown's selected value
        if (index < 0 || index >= filteredResolutions.Count) return;

        Resolution res = filteredResolutions[index];

        // Apply
        Screen.SetResolution(res.width, res.height, Screen.fullScreenMode);

        Debug.Log($"SUCCESS: Set to {res.width} x {res.height} at index {index}");
    }
}