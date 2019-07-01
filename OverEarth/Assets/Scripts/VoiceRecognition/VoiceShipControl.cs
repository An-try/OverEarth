using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceShipControl : MonoBehaviour
{
    public static VoiceShipControl instance;

    public delegate void OnCommandExecuted();
    public OnCommandExecuted onCommandExecuted;

    public delegate void OnVoiceShipControlTurnedOnOff(bool value);
    public OnVoiceShipControlTurnedOnOff onVoiceShipControlTurnedOnOff;

    private Ship playerShipScript;

    public KeywordRecognizer keywordRecognizer;

    private List<string> AllCommands = new List<string>();

    public Dictionary<string, Action> CurrentChoiceActions = new Dictionary<string, Action>();
    
    private Dictionary<string, Action> MainActions = new Dictionary<string, Action>();
    private Dictionary<string, Action> ShipActions = new Dictionary<string, Action>();
    private Dictionary<string, Action> MoveDirectionActions = new Dictionary<string, Action>();
    private Dictionary<string, Action> SetSpeedActions = new Dictionary<string, Action>();
    private Dictionary<string, Action> InventoryActions = new Dictionary<string, Action>();

    public Dictionary<string, Action> DefaultActions = new Dictionary<string, Action>();

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

        playerShipScript = PlayerMovement.instance.ShipScript;

        // Задаём команды для голосового управления
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

        // Добавляем фразы для распознавания в один список
        AllCommands.AddRange(MainActions.Keys);
        AllCommands.AddRange(ShipActions.Keys);
        AllCommands.AddRange(MoveDirectionActions.Keys);
        AllCommands.AddRange(SetSpeedActions.Keys);
        AllCommands.AddRange(InventoryActions.Keys);

        CurrentChoiceActions = MainActions; // Задать на прослушивание главные команды
        onCommandExecuted?.Invoke();

        try
        {
            keywordRecognizer = new KeywordRecognizer(AllCommands.ToArray()); // Создать разпознаватель слов с массивом фраз для распознавания

            keywordRecognizer.OnPhraseRecognized += SpeechRecognize; // Если услышали что-нибудь(OnPhraseRecognized), вызвать метод разпознавания фразы(SpeechRecognize)
        }
        catch
        {
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
            keywordRecognizer.Start(); // Начать распознавание речи
        }
        else
        {
            keywordRecognizer.Stop();
        }
        onVoiceShipControlTurnedOnOff?.Invoke(keywordRecognizer.IsRunning);
    }

    private void SpeechRecognize(PhraseRecognizedEventArgs speech)
    {
        if (CurrentChoiceActions.ContainsKey(speech.text))
        {
            CurrentChoiceActions[speech.text].Invoke(); // Вызвать метод выполнения команды по сказаной фразе
            onCommandExecuted?.Invoke();
        }
        if (DefaultActions.ContainsKey(speech.text))
        {
            DefaultActions[speech.text].Invoke();
            onCommandExecuted?.Invoke();
        }
    }

    private void OnDestroy()
    {
        if (keywordRecognizer != null)
        {
            if (keywordRecognizer.IsRunning)
            {
                keywordRecognizer.Stop();
            }

            keywordRecognizer.Dispose();
        }
    }

    // Описание методов выполнения команд
    private void MainAction()
    {
        CurrentChoiceActions = MainActions;
    }

    private void ShipAction()
    {
        CurrentChoiceActions = ShipActions;
    }

    private void MoveDirectionAction()
    {
        CurrentChoiceActions = MoveDirectionActions;
    }

    private void ForwardDirectionAction()
    {
        playerShipScript.transmission = 4;
    }

    private void BackDirectionAction()
    {
        playerShipScript.transmission = -4;
    }

    private void RightDirectionAction()
    {
        // TODO: Rotate ship right
        playerShipScript.transmission = 4;
    }

    private void LeftDirectionAction()
    {
        // TODO: Rotate ship left
        playerShipScript.transmission = -4;
    }

    private void StopShipAction()
    {
        playerShipScript.transmission = 0;
    }

    private void InventoryAction()
    {
        CurrentChoiceActions = InventoryActions;
    }

    private void InventoryInteractAction()
    {
        Manager.instance.ActivateDeactivateUIPanel(Manager.instance.InventoryPanel);
    }

    private void CancelCommandAction()
    {
        CurrentChoiceActions = MainActions;
    }
}
