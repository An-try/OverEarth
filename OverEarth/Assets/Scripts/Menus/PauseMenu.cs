using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;  // Singleton for this script

    public GameObject _PauseMenu;

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

    public void PauseMenuInteraction()
    {
        if (Time.timeScale == 0) // If game is paused
        {
            ResumeGame();
        }
        else // If game is not paused
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        Cursor.visible = true;
        _PauseMenu.SetActive(true);
        Time.timeScale = 0f; // Set game speed to 0 (pause game)

        Manager.instance.GameInfoPanel.SetActive(false); // Deactivate main game info panel

        // Disable all UI panels
        foreach (GameObject Panel in Manager.instance.UIPanels)
        {
            Panel.SetActive(false);
        }

        if (Manager.instance.CurrentTargetInfoCamera) // If current target info camera exists
        {
            Manager.instance.CurrentTargetInfoCamera.SetActive(false); // Disable current target info camera
        }
    }

    public void ResumeGame()
    {
        Cursor.visible = false;
        _PauseMenu.SetActive(false);
        Time.timeScale = 1f; // Set game speed to 1 (normal game speed)

        Manager.instance.GameInfoPanel.SetActive(true); // Activate main game info panel

        if (Manager.instance.CurrentTargetInfoCamera) // If target info camera exists
        {
            Manager.instance.CurrentTargetInfoCamera.SetActive(true); // Activate current target info camera
        }
    }

    public void ExitGame()
    {
        ResumeGame();
        SceneManager.LoadSceneAsync("Menu"); // Loads a new scene in the background without closing the current scene
    }
}
