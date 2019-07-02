using UnityEngine;
using UnityEngine.UI;

public class VoiceCommandsInfoPanel : MonoBehaviour
{
    public static VoiceCommandsInfoPanel instance; // Singleton for this script

    public Text infoText; // Text that displays whether voice control is enabled
    public Text currentAvaliableCommandsText; // Displays current available commands

    private VoiceShipControl voiceShipControl;

    private void Awake() // Awake is called when the script instance is being loaded
    {
        if (instance == null) // If instance not exist
        {
            instance = this; // Set up instance as this script
        }
        else //If instance already exists
        {
            Destroy(this); // Destroy this script
        }
    }

    private void Start() // Start is called on the frame when a script is enabled just before any of the Update methods are called the first time
    {
        Manager.instance.onPlayerShipAssigned += SetVoiceShipControl; // Delegate called when player ship is assigned
    }

    private void SetVoiceShipControl()
    {
        voiceShipControl = VoiceShipControl.instance; // Get the instance of voice ship control

        voiceShipControl.onCommandExecuted += ChangeAvaliableCommands; // Delegate called when any command executes
        voiceShipControl.onVoiceShipControlTurnedOnOff += ChangeInfoText; // Delegate called when voice ship control was turned on or off

        voiceShipControl.onCommandExecuted?.Invoke(); // Call the command execution delegate to refresh the panel of available commands

        if (voiceShipControl.keywordRecognizer != null) // If there is a keyword recognizer
        {
            // Call a delegate that changes text if keyword recognizer is running
            voiceShipControl.onVoiceShipControlTurnedOnOff?.Invoke(voiceShipControl.keywordRecognizer.IsRunning);
        }
    }

    private void ChangeAvaliableCommands()
    {
        currentAvaliableCommandsText.text = null; // Delete text from the text component

        foreach (string command in voiceShipControl.CurrentAvailableCommands.Keys) // Pass all current available commands
        {
            currentAvaliableCommandsText.text += command + "\n"; // Display this commands
        }

        foreach (string command in voiceShipControl.DefaultActions.Keys) // Pass all default commands
        {
            currentAvaliableCommandsText.text += "\n" + command; // Display this commands
        }
    }

    private void ChangeInfoText(bool voiceRecognitionEnabled)
    {
        // Display if voice recognition is on or off
        if (voiceRecognitionEnabled)
        {
            infoText.text = "Voice commands: <color=lime>" + voiceRecognitionEnabled + "</color>";
        }
        else
        {
            infoText.text = "Voice commands: <color=red>" + voiceRecognitionEnabled + "</color>";
        }
    }
}
