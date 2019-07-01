﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manager for game scene.
/// </summary>
public class Manager : MonoBehaviour
{
    // Awake() // Awake is called when the script instance is being loaded
    // Start() // Start is called on the frame when a script is enabled just before any of the Update methods are called the first time
    // Update() // Update is called every frame
    // FixedUpdate() // FixedUpdate is called at a fixed framerate frequency
    // OnEnable() // Called when the object becomes enabled and active
    // OnCollisionEnter(Collision collision) // Called when this collider/rigidbody has begun touching another rigidbody/collider
    // base.SomeMethod(); // Calling the base class of this method

    // TODO: Make an object pooling

    public static Manager instance; // Singleton for this script

    private int currentFPS; // Current frames per second

    // Delegate when player ship spawned
    public delegate void OnPlayerShipAssigned();
    public OnPlayerShipAssigned onPlayerShipAssigned;

    // Prefab of any ship in the game
    // There is only one kind of ship in the game
    public GameObject ShipPrefab;

    public GameObject FPSInfo; // Object with info about current FPS
    public GameObject WarningMessageObject; // Object that shows any warning message

    // Turret prefabs
    public GameObject LaserPulseTurretPrefab;
    public GameObject PlasmaTurretPrefab;
    public GameObject RocketTurretPrefab;
    
    public GameObject StartStation; // Start game station
    public GameObject PlayerShip;

    public GameObject PlayerShipInfoCanvas; // Main canvas that contains all player ship UI panels
    public GameObject GameInfoPanel;
    public GameObject InventoryPanel;
    public GameObject HelpPanel; // Panel that contains player input help and other help information
    
    // TODO: Remake target camera algorithm
    public GameObject TargetInfoCameraPrefab; // Camera prefab that shows current selected target
    public GameObject CurrentTargetInfoCamera; // Camera that shows current selected target
    public GameObject CurrentSelectedTarget = null;
    public GameObject LastSelectedTarget = null;

    public List<GameObject> GameCameras = new List<GameObject>(); // List with all cameras that tagged "MainCamera" on the scene 
    public List<GameObject> UIPanels = new List<GameObject>(); // List with all UI panels that tagged "UIPanel" on the scene

    // Lists with all current existing Allies and Enemies on the scene
    public static List<GameObject> Enemies = new List<GameObject>();
    public static List<GameObject> Allies = new List<GameObject>();

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
        // Add all cameras with tag "MainCamera" to game cameras array
        foreach (GameObject Camera in GameObject.FindGameObjectsWithTag("MainCamera"))
        {
            GameCameras.Add(Camera);
        }

        // Add all UI panels with tag "UIPanel" to UI panels array
        foreach (GameObject UIPanel in GameObject.FindGameObjectsWithTag("UIPanel"))
        {
            UIPanels.Add(UIPanel);
        }

        // Set all game cameras unactive except station camera
        for (int i = 0; i < GameCameras.Count; i++)
        {
            if (GameCameras[i].name == "StationCamera")
            {
                GameCameras[i].SetActive(true);
            }
            else
            {
                GameCameras[i].SetActive(false);
            }
        }

        // Set all UI panels unactive
        foreach (GameObject Panel in UIPanels)
        {
            Panel.SetActive(false);
        }

        InvokeRepeating("UpdateCurrentFPSInfo", 0, 0.5f); // Update FPS info info every 0.5 seconds

        // Spawn an enemy ship on the game object named "ShipSpawn"
        // TODO: Remake enemies spawn

        //SpawnShip("Enemy", GameObject.Find("ShipSpawn").transform.position, GameObject.Find("ShipSpawn").transform.localRotation);
        //SpawnShip("Enemy", GameObject.Find("ShipSpawn1").transform.position, GameObject.Find("ShipSpawn1").transform.localRotation);
        //SpawnShip("Enemy", GameObject.Find("ShipSpawn2").transform.position, GameObject.Find("ShipSpawn2").transform.localRotation);
    }

    private void Update() // Update is called every frame
    {
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            SpawnShip("Enemy", GameObject.Find("ShipSpawn").transform.position, GameObject.Find("ShipSpawn").transform.localRotation);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SpawnShip("Enemy", GameObject.Find("ShipSpawn1").transform.position, GameObject.Find("ShipSpawn1").transform.localRotation);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SpawnShip("Enemy", GameObject.Find("ShipSpawn2").transform.position, GameObject.Find("ShipSpawn2").transform.localRotation);
        }

        currentFPS = (int)(1f / Time.unscaledDeltaTime); // Get current frames per second

        // If the current target info camera has destroyed, spawn a new target info camera, transform and parent it to player ship
        // TODO: Remake target camera algorithm
        if (CurrentTargetInfoCamera == null && PlayerShip != null)
        {
            GameObject cameraTemp = Instantiate(TargetInfoCameraPrefab, PlayerShip.transform);
            cameraTemp.GetComponent<TargetInfoCamera>().PlayerShip = PlayerShip;
            cameraTemp.transform.SetParent(PlayerShip.transform);
            CurrentTargetInfoCamera = cameraTemp;
        }

        if (Time.timeScale == 1) // If game is not paused
        {
            // Open or close player ship inventory
            if (Input.GetKeyDown(KeyCode.I) && PlayerShip)
            {
                Inventory.instance.InventoryPanelExecute();
            }

            // Open help panel that contains player input help and other help information
            if (Input.GetKeyDown(KeyCode.P) && PlayerShip) // If the "P" key is pressed
            {
                ActivateDeactivateUIPanel(HelpPanel);
            }
            
            if (Input.GetKeyDown(KeyCode.L) && PlayerShip) // If the "L" key is pressed and player ship exists
            {
                ChangeGameCamera(); // Change camera in the game
            }
            
            if (Input.GetKeyDown(KeyCode.G) && PlayerShip) // If the "G" key is pressed and player ship exists
            {
                EquipmentManager.instance.UnequipAllEquipment(); // Unequip all equipment from player ship
            }

            // If the "V" key is pressed and player ship exists and exists an instance of VoiceCommandsInfoPanel script
            // Start or stop voice ship control
            if (Input.GetKeyDown(KeyCode.V) && PlayerShip && VoiceCommandsInfoPanel.instance)
            {
                VoiceShipControl.instance.RunKeywordRecognizer(); // Run or disable keyword recognizer
            }
        }

        // If the left "Alt" key is pressed
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Cursor.visible = !Cursor.visible; // Show or hide cursor
        }

        // If the "Escape" key is pressed
        // Pause the game and open pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu.instance.PauseMenuInteraction(); // Enable or disable pause menu
        }
    }

    /// <summary>
    /// Updates info about current FPS on the FPSInfo game object.
    /// </summary>
    private void UpdateCurrentFPSInfo()
    {
        // Update current FPS on the text component of the FPSinfo object
        FPSInfo.GetComponent<Text>().text = "<color=magenta>FPS: " + currentFPS + "</color>";
    }

    // On the start of the game deactivate button that call player ship spawn
    // And activate info text that describes how to change camera from station camera to player ship camera
    public void DEACTIVATE_BUTTON(GameObject Button)
    {
        Button.SetActive(false);
    }
    public void ACTIVATE_TEXT (GameObject InfoText)
    {
        InfoText.SetActive(true);
    }

    // Spawn a player ship on the manager game object position and rotation
    public void SpawnPlayer()
    {
        SpawnShip("Player", transform.position, transform.rotation);
    }

    // Spawn a ship with tag, coordinates and rotation
    public void SpawnShip(string shipTag, Vector3 spawnCoordinates, Quaternion spawnRotation)
    {
        GameObject Ship = Instantiate(ShipPrefab, spawnCoordinates, spawnRotation); // Instantiate a new ship game object
        Ship.tag = shipTag; // Set tag to a new ship

        // Check ship's tag and perform certain actions depending on this tag
        switch (shipTag)
        {
            case "Player":
                ActionsForPlayerShipSpawn(Ship);
                break;
            case "Ally":
                ActionsForAllyShipSpawn(Ship);
                break;
            case "Enemy":
                ActionsForEnemyShipSpawn(Ship);
                break;
            default:
                break;
        }
    }

    private void ActionsForPlayerShipSpawn(GameObject Ship)
    {
        PlayerShip = Ship; // Set the main player ship

        PlayerShip.GetComponent<Ship>().HP = 100000f;
        
        // Add scripts of player movement and voice ship control
        PlayerShip.AddComponent<PlayerMovement>();
        PlayerShip.AddComponent<VoiceShipControl>();
        PlayerShip.AddComponent<Inventory>();

        PlayerShipCameraController.instance.PlayerShip = PlayerShip; // Set up an player ship in camera controller
        // Set an camera target game object
        PlayerShipCameraController.instance.TargetCameraRotatesAround = PlayerShip.GetComponent<Ship>().TargetCameraRotatesAround;

        Allies.Add(PlayerShip); // Add a player ship to allies list

        onPlayerShipAssigned?.Invoke(); // Invoke a delegate if there is any signature on it
    }

    private void ActionsForAllyShipSpawn(GameObject Ship)
    {
        Allies.Add(Ship); // Add a ally ship to allies list
    }

    private void ActionsForEnemyShipSpawn(GameObject Ship)
    {
        Enemies.Add(Ship); // Add an emeny ship to enemies list
    }
    
    private void ChangeGameCamera() // Activate new camera and deactivate old camera
    {
        for (int i = 0; i < GameCameras.Count; i++) // Check all cameras in GameCameras list
        {
            if (GameCameras[i].activeSelf) // If the camera is active
            {
                GameCameras[i].SetActive(false); // Deactivate this camera

                // Activate next camera in the cameras list
                if (i + 1 < GameCameras.Count) // If the next camera exists
                {
                    GameCameras[i + 1].SetActive(true); // Activate next camera
                }
                else // If the next camera doesn't exists
                {
                    GameCameras[0].SetActive(true); // Activate first camera in the list
                }
                return;
            }
        }
    }
    
    public void ActivateDeactivateUIPanel(GameObject PanelToActivateOrDeactivate) // Activate or deactivate UI panel which is transmitted as a property
    {
        foreach (GameObject Panel in UIPanels)  // Check all panels in UIPanels list
        {
            if (PanelToActivateOrDeactivate == Panel) // If the panel that needs to be activated equals to the achieved panel in the list
            {
                if (Panel.activeSelf) // If this panel is active
                {
                    Panel.SetActive(false); // Deactivate this panel
                    GameInfoPanel.SetActive(true); // Activate main game info panel
                    CurrentTargetInfoCamera.SetActive(true); // Activate current target info camera
                }
                else // If this panel isn't active
                {
                    Panel.SetActive(true); // Activate this panel
                    GameInfoPanel.SetActive(false); // Deactivate main game info panel
                    CurrentTargetInfoCamera.SetActive(false); // Deactivate current target info camera
                }
            }
            else // If the achieved panel in the list not equals to panel that needs to be activated
            {
                Panel.SetActive(false); // Deactivate this panel
            }
        }
    }
}
