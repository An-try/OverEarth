using UnityEngine;
using UnityEngine.UI;

public class VoiceCommandsInfoPanel : MonoBehaviour
{
    public static VoiceCommandsInfoPanel instance;

    public Text infoText;
    public Text currentAvaliableCommandsText;

    private VoiceShipControl voiceShipControl;

    private void Awake()
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

    private void Start()
    {
        Manager.instance.onPlayerShipAssigned += SetVoiceShipControl;
    }

    private void SetVoiceShipControl()
    {
        voiceShipControl = VoiceShipControl.instance;

        voiceShipControl.onCommandExecuted += ChangeAvaliableCommands;
        voiceShipControl.onVoiceShipControlTurnedOnOff += ChangeInfoText;

        voiceShipControl.onCommandExecuted?.Invoke();

        if (voiceShipControl.keywordRecognizer != null)
        {
            voiceShipControl.onVoiceShipControlTurnedOnOff?.Invoke(voiceShipControl.keywordRecognizer.IsRunning);
        }
    }

    private void ChangeAvaliableCommands()
    {
        currentAvaliableCommandsText.text = null;

        foreach (string command in voiceShipControl.CurrentChoiceActions.Keys)
        {
            currentAvaliableCommandsText.text += command + "\n";
        }
        foreach (string command in voiceShipControl.DefaultActions.Keys)
        {
            currentAvaliableCommandsText.text += "\n" + command;
        }
    }

    private void ChangeInfoText(bool value)
    {
        if (value)
        {
            infoText.text = "Voice commands: <color=lime>" + value + "</color>";
        }
        else
        {
            infoText.text = "Voice commands: <color=red>" + value + "</color>";
        }
    }
}
