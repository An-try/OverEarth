using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    private string optionsFilePath = "PlayerOptions.txt"; // Path to the settings file where all settings are saved

    public GameObject WarningMessageObject; // Object that shows any warning message
    
    public Dropdown resolutionsDropdown;

    public GameObject FullScreenCheckMark;
    
    public AudioMixer audioMixer; // Audio mixer that controls sounds in the game
    public Slider volumeSlider;
    
    private int[,] availableResolutions; // Array with available screen resolutions
    
    private void Awake() // Awake is called when the script instance is being loaded
    {
        WarningMessageObject.SetActive(false);

        // Available screen resolutions in the game
        availableResolutions = new int[,]{ { 800, 1024, 1280, 1280, 1280, 1280, 1366, 1600, 1920, 7680 },
                                           { 600,  768,  600,  720,  768, 1024,  768,  900, 1080, 4800 } };
        
        List<string> resolutionsToDropdown = new List<string>(); // Resolutions that needs to be added to a dropdown

        // Add all resolutions to list
        for (int i = 0; i < availableResolutions.Length / 2; i++)
        {
            // Add resolution from first and second rows of resolutions array
            // It looks like this: 800x600
            resolutionsToDropdown.Add(availableResolutions[0, i] + "x" + availableResolutions[1, i]);
        }

        resolutionsDropdown.AddOptions(resolutionsToDropdown); // Add resolutions to dropdown from the list

        NewSettings.resolution = new Vector2Int(Screen.currentResolution.width, Screen.currentResolution.height); // Set current screen width and height
        NewSettings.fullScreen = true; // Set current full screen
        NewSettings.volumeValue = 0; // Set current volume

        if (!File.Exists(optionsFilePath)) // If the options file does not exist
        {
            NewSettings.resolution = GetMaximumAvailableResolution(); // Define and set the optimal screen resolution
            Screen.SetResolution(GetMaximumAvailableResolution().x, GetMaximumAvailableResolution().y, Screen.fullScreen); // Set current screen resolution
            WriteCurrentSettingsToFile();
        }
        else
        {
            ReadSettingsFromFile();
            ApplyNewSettings();
        }
        UpdateSettingsOnPanel(); // Change values on the settings panel depending on the current game settings
    }

    private void OnEnable() // Called when the object becomes enabled and active
    {
        ReadSettingsFromFile();
        ApplyNewSettings();
        UpdateSettingsOnPanel(); // Change values on the settings panel depending on the current game settings
    }

    /// <summary>
    /// Class for saving new settings that have been changed but needs to be applied.
    /// </summary>
    private static class NewSettings
    {
        public static Vector2Int resolution;
        public static bool fullScreen;
        public static float volumeValue;
    }

    private void WriteCurrentSettingsToFile()
    {
        // Create an array of string settings to write to the file
        string[] settings = { "Resolution: " + NewSettings.resolution.x + "x" + NewSettings.resolution.y,
                              "Full screen: " + NewSettings.fullScreen,
                              "Volume: " + NewSettings.volumeValue };

        File.WriteAllLines(optionsFilePath, settings); // Create a file and write current settings
    }

    private void ReadSettingsFromFile()
    {
        string[] lines = File.ReadAllLines(optionsFilePath); // Create array of strings and read data from settings file

        if (lines[0].Contains("Resolution: ")) // If the first line of settings contains "Resolution: "
        {
            lines[0] = lines[0].Replace("Resolution: ", ""); // Delete "Resolution: " from string
            // Split this line to two strings by 'x'. At the output we will get two strings with saved screen width and height
            string[] resolutions = lines[0].Split('x');

            if (int.TryParse(resolutions[0], out int width) && int.TryParse(resolutions[1], out int height)) // Check if these strings are numeric
            {
                NewSettings.resolution = new Vector2Int(width, height); // Set new resolution
            }
            else // If any string is not numeric
            {
                NewSettings.resolution = GetMaximumAvailableResolution(); // Set new resolution depending on maximum available resolution
            }
        }
        else // If the first line of settings does not contain "Resolution: "
        {
            NewSettings.resolution = GetMaximumAvailableResolution(); // Set new resolution depending on maximum available resolution
        }

        if (lines[1].Contains("Full screen: ")) // If the second line of settings contains "Full screen: "
        {
            lines[1] = lines[1].Replace("Full screen: ", ""); // Delete "Full screen: " from string

            if (bool.TryParse(lines[1], out bool fullScreen)) // Check if this strings is boolean
            {
                NewSettings.fullScreen = fullScreen; // Set new full screen
            }
            else // If this string is not boolean
            {
                NewSettings.fullScreen = Screen.fullScreen; // Set full screen depending on current full screen
            }
        }
        else // If the second line of settings does not contain "Full screen: "
        {
            NewSettings.fullScreen = Screen.fullScreen; // Set full screen depending on current full screen
        }

        if (lines[2].Contains("Volume: ")) // If the third line of settings contains "Volume: "
        {
            lines[2] = lines[2].Replace("Volume: ", ""); // Delete "Volume: " from string

            if (float.TryParse(lines[2], out float volume)) // Check if this strings is float
            {
                NewSettings.volumeValue = volume; // Set new volume
            }
            else // If this string is not float
            {
                NewSettings.volumeValue = GetCurrentAudioVolume(); // Set volume depending on current volume
            }
        }
        else // If the third line of settings does not contain "Volume: "
        {
            NewSettings.volumeValue = GetCurrentAudioVolume(); // Set volume depending on current volume
        }
    }

    /// <summary>
    /// Update current settings on the settings panel.
    /// </summary>
    private void UpdateSettingsOnPanel()
    {
        // Get current program parameters and set them to variables
        SetCurrentResolutionInDropdown();
        FullScreenCheckMark.SetActive(NewSettings.fullScreen); // Set fullscreen check mark depending on current fullscreen
        volumeSlider.value = NewSettings.volumeValue; // Set volume slider value depending on current volume value
    }

    /// <summary>
    /// Defines nearest or equal screen resolution in available resolutions depending on current desktop resolution.
    /// </summary>
    private Vector2Int GetMaximumAvailableResolution()
    {
        int currentDesktopScreenWidth = Screen.currentResolution.width;
        int currentDesktopScreenHeight = Screen.currentResolution.height;

        int newScreenWidth = currentDesktopScreenWidth;
        int newScreenHeight = currentDesktopScreenHeight;

        for (int width = 0; width < availableResolutions.Length / 2; width++) // Check all widths in available resolutions (in the first row of array)
        {
            if (availableResolutions[0, width] == currentDesktopScreenWidth) // If width in array is equal to current screen width
            {
                newScreenWidth = currentDesktopScreenWidth; // Set this width as new
                break; // Stop for loop
            }

            // This if operator choose the nearest smaller width depending on current screen width if the equal width will not be found
            if (availableResolutions[0, width] <= currentDesktopScreenWidth) // If width in array is less than current screen width
            {
                newScreenWidth = availableResolutions[0, width]; // Set this width as new
            }
        }

        for (int height = 0; height < availableResolutions.Length / 2; height++) // Check all heights in available resolutions (in the second row of array)
        {
            if (availableResolutions[1, height] == currentDesktopScreenHeight) // If height in array is equal to current screen height
            {
                newScreenHeight = currentDesktopScreenHeight; // Set this height as new
                break; // Stop for loop
            }

            // This if operator choose the nearest smaller height depending on current screen height if the equal height will not be found
            if (availableResolutions[1, height] <= currentDesktopScreenHeight) // If height in array is less than current screen height
            {
                newScreenHeight = availableResolutions[1, height]; // Set this height as new
            }
        }

        return new Vector2Int(newScreenWidth, newScreenHeight);
    }

    private void SetCurrentResolutionInDropdown()
    {
        for (int i = 0; i < resolutionsDropdown.options.Count; i++) // Pass all resolutions in the drop-down list
        {
            // If the resolution in the drop-down list coincided with the current screen resolution
            if (resolutionsDropdown.options[i].text == string.Format(NewSettings.resolution.x + "x" + NewSettings.resolution.y))
            {
                resolutionsDropdown.value = i; // Select the current screen resolution in the drop-down list
            }
        }
    }

    /// <summary>
    /// Returns the maximum resolution supported by the monitor.
    /// </summary>
    /// <returns></returns>
    private Vector2Int GetMaximumSupportedResolution()
    {
        Resolution[] monitorResolutions = Screen.resolutions; // Get array of full-screen resolutions supported by the monitor
        // Get the higest(last) screen resolution in array
        Vector2Int highestResolution =
            new Vector2Int(monitorResolutions[monitorResolutions.Length - 1].width, monitorResolutions[monitorResolutions.Length - 1].height);

        return highestResolution;
    }

    private float GetCurrentAudioVolume()
    {
        // If there is a "Volume" parameter in the audio mixer, transfer the volume value to "volumeValue"
        bool valueExist = audioMixer.GetFloat("Volume", out float volumeValue);

        if (valueExist)
        {
            return volumeValue; // Return current volume in audio mixer
        }
        return 0; // Return 0 if the current volume was not found
    }

    public void SetScreenResolutionToNewTempSettings() // Called when choosing any new resolution in the dropdown list in options menu
    {
        // Get the new chosen width and height in dropdown as array of strings
        // It takes current resolutionsDropdown.value as an index of current chosen dropdown option. Then this option splits by 'x'
        // And at the output we get current width and height, which are selected in the dropdown
        string[] currentDropdownResolution = resolutionsDropdown.options[resolutionsDropdown.value].text.Split('x');

        // If chosen resolution in dropdown is less or equal to the maximum supported resolution
        if (int.Parse(currentDropdownResolution[0]) <= GetMaximumSupportedResolution().x &&
            int.Parse(currentDropdownResolution[1]) <= GetMaximumSupportedResolution().y)
        {
            NewSettings.resolution = new Vector2Int(int.Parse(currentDropdownResolution[0]), int.Parse(currentDropdownResolution[1]));
        }
        else // If chosen resolution in dropdown is higher than maximum supported resolution
        {
            // Activate warning text that notify the user about maximum supported resolution
            StartCoroutine(Methods.ShowWarningMessage(WarningMessageObject,
                                                      "Maximum supported resolution is: <color=orange>" + GetMaximumSupportedResolution().x + "x" + GetMaximumSupportedResolution().y + "</color>",
                                                      5f));
            SetCurrentResolutionInDropdown(); // Set the previous resolution option in dropdown
        }
    }

    public void SetFullScreenToNewTempSettings() // Called when changing full screen option in the options menu
    {
        FullScreenCheckMark.SetActive(!FullScreenCheckMark.activeSelf); // Put full screen check mark in another active state
        NewSettings.fullScreen = FullScreenCheckMark.activeSelf; // Set the new full screen settings
    }

    public void SetVolumeToNewTempSettings(float volumeValue) // Called when changing volume value in the options menu
    {
        NewSettings.volumeValue = volumeValue; // Set new volume with the slider in the settings
    }

    public void ApplySettingsButtonPressed()
    {
        ApplyNewSettings();
        WriteCurrentSettingsToFile();
        UpdateSettingsOnPanel(); // Change values on the settings panel depending on the current game settings
    }

    private void ApplyNewSettings()
    {
        // If current screen resolution is not equal to the new screen resolution
        if (Screen.width != NewSettings.resolution.x || Screen.height != NewSettings.resolution.y)
        {
            // Set new resolution and don't change full screen
            Screen.SetResolution(NewSettings.resolution.x, NewSettings.resolution.y, Screen.fullScreen);
        }

        if (Screen.fullScreen != NewSettings.fullScreen) // If current full screen is not equal to the new full screen option
        {
            Screen.fullScreen = NewSettings.fullScreen; // Set new full screen option
        }

        if (GetCurrentAudioVolume() != NewSettings.volumeValue) // If current volume is not equal to the new volume value
        {
            audioMixer.SetFloat("Volume", NewSettings.volumeValue); // Set new volume value
        }
    }
}
