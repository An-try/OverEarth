using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceShipControl : MonoBehaviour
{
    public static VoiceShipControl instance; // Singleton for this script

    // Delegate called when any command is executed
    public delegate void OnCommandExecuted();
    public OnCommandExecuted onCommandExecuted;

    // Delegate called when turning on or off voice ship control
    public delegate void OnVoiceShipControlTurnedOnOff(bool value);
    public OnVoiceShipControlTurnedOnOff onVoiceShipControlTurnedOnOff;

    private Ship playerShipScript;

    public KeywordRecognizer keywordRecognizer;

    private List<string> AllCommands = new List<string>();
    
    public Dictionary<string, Action> CurrentAvailableCommands = new Dictionary<string, Action>();
    
    private Dictionary<string, Action> MainActions = new Dictionary<string, Action>();
    private Dictionary<string, Action> ShipActions = new Dictionary<string, Action>();
    private Dictionary<string, Action> MoveDirectionActions = new Dictionary<string, Action>();
    private Dictionary<string, Action> SetSpeedActions = new Dictionary<string, Action>();
    private Dictionary<string, Action> InventoryActions = new Dictionary<string, Action>();

    public Dictionary<string, Action> DefaultActions = new Dictionary<string, Action>();

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

        playerShipScript = PlayerMovement.instance.shipScript; // Get the player ship script

        // Set the commands for voice control
        MainActions.Add("ship", ShipAction);
            ShipActions.Add("move", MoveDirectionAction);
                MoveDirectionActions.Add("forward", ForwardDirectionAction);
                MoveDirectionActions.Add("backward", BackDirectionAction);
                MoveDirectionActions.Add("right", RightDirectionAction);
                MoveDirectionActions.Add("left", LeftDirectionAction);
                MoveDirectionActions.Add("stop", StopShipAction);
            ShipActions.Add("inventory", InventoryAction);
                InventoryActions.Add("interact", InventoryInteractAction);
        DefaultActions.Add("main", CancelCommandAction);

        // Add phrases for recognition in one list
        AllCommands.AddRange(MainActions.Keys);
        AllCommands.AddRange(ShipActions.Keys);
        AllCommands.AddRange(MoveDirectionActions.Keys);
        AllCommands.AddRange(SetSpeedActions.Keys);
        AllCommands.AddRange(InventoryActions.Keys);

        CurrentAvailableCommands = MainActions; // Check the main commands
        onCommandExecuted?.Invoke(); // Call a delegate of command executing to update the available commands on UI panel

        // This try-catch block catches an error when speech recognition is not supported on this computer
        try
        {
            keywordRecognizer = new KeywordRecognizer(AllCommands.ToArray()); // Create a word recognizer with an array of phrases to recognize
            // Delegate call when some phrase has been recognized
            keywordRecognizer.OnPhraseRecognized += RecognizeSpeech;
        }
        catch // If speech recognition is not supported on this computer
        {
            // Show warning message, and destroy warning message, voice commands info panel and this script after a while
            float timeToDestroy = 10f;
            StartCoroutine(
                Methods.ShowWarningMessage(Manager.instance.WarningMessageObject, "Speech recognition is not supported on this computer", timeToDestroy));
            Destroy(VoiceCommandsInfoPanel.instance.gameObject);
            Destroy(this, timeToDestroy + 1f);
        }
    }

    public void RunKeywordRecognizer()
    {
        if (!keywordRecognizer.IsRunning)
        {
            keywordRecognizer.Start(); // Start speech recognition
        }
        else
        {
            keywordRecognizer.Stop(); // Stop speech recognition
        }

        onVoiceShipControlTurnedOnOff?.Invoke(keywordRecognizer.IsRunning);
    }

    private void RecognizeSpeech(PhraseRecognizedEventArgs phrase)
    {
        if (CurrentAvailableCommands.ContainsKey(phrase.text)) // If available commands contains current above phrase
        {
            CurrentAvailableCommands[phrase.text].Invoke(); // Call the command execution method for the above phrase
            onCommandExecuted?.Invoke();
        }

        if (DefaultActions.ContainsKey(phrase.text)) // If default commands contains current said phras
        {
            DefaultActions[phrase.text].Invoke(); // Call the command execution method for the above phrase
            onCommandExecuted?.Invoke();
        }
    }

    private void OnDestroy() // Called when destroying this script
    {
        if (keywordRecognizer != null) // If there is any keyword recognizer
        {
            if (keywordRecognizer.IsRunning) // If it is running
            {
                keywordRecognizer.Stop(); // Stop this keyword recognizer
            }

            keywordRecognizer.Dispose(); // Dispose this keyword recognizer
        }
    }
    
    private void MainAction() // Change available commands
    {
        CurrentAvailableCommands = MainActions;
    }

    private void ShipAction() // Change available commands
    {
        CurrentAvailableCommands = ShipActions;
    }

    private void MoveDirectionAction() // Change available commands
    {
        CurrentAvailableCommands = MoveDirectionActions;
    }

    private void ForwardDirectionAction() // Move player ship forward
    {
        playerShipScript.transmission = 4;
    }

    private void BackDirectionAction() // Move player ship backward
    {
        playerShipScript.transmission = -4;
    }

    private void RightDirectionAction() // Rotate player ship right
    {
        // TODO: Rotate ship right
    }

    private void LeftDirectionAction() // Rotate player ship left
    {
        // TODO: Rotate ship left
    }

    private void StopShipAction()
    {
        playerShipScript.transmission = 0;
    }

    private void InventoryAction() // Change available commands
    {
        CurrentAvailableCommands = InventoryActions;
    }

    private void InventoryInteractAction() // Open or close inventory panel
    {
        Manager.instance.ActivateDeactivateUIPanel(Manager.instance.InventoryPanel);
    }

    private void CancelCommandAction() // Change available commands. Executes when player wants to return to the main commands
    {
        CurrentAvailableCommands = MainActions;
    }
}
