using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject OptionsMenu;

    // Objects that shows progress of loading a new scene
    public GameObject LoadingInfoObject;
    public Text LoadingProgressText;
    public Slider loadingProgressBar;

    private void Awake() // Awake is called when the script instance is being loaded
    {
        MainMenu.SetActive(true);
        OptionsMenu.SetActive(false);
        LoadingInfoObject.SetActive(false);
    }

    private void Start() // Start is called on the frame when a script is enabled just before any of the Update methods are called the first time
    {
        Cursor.visible = true;
    }

    public void PlayButtonPressed() // The game start button is pressed
    {
        MainMenu.SetActive(false); // Deactivate the main menu
        LoadSceneAsynchronously("Game"); // Load game scene
    }

    public void OptionsButtonPressed()
    {
        OptionsMenu.SetActive(true); // Activate the options window
        MainMenu.SetActive(false); // Deactivate the main menu
    }

    public void CloseOptionsButtonPressed()
    {
        MainMenu.SetActive(true); // Activate the main menu
        OptionsMenu.SetActive(false); // Deactivate the options window
    }

    public void ExitButtonPressed() // Application exit button pressed
    {
        Application.Quit(); // Exit application
    }

    /// <summary>
    /// Starts a coroutine that loads scene asynchronously.
    /// </summary>
    /// <param name="sceneName"></param>
    private void LoadSceneAsynchronously(string sceneName)
    {
        StartCoroutine(LoadAsynchronously(sceneName));
    }

    /// <summary>
    /// Load a new scene in the background without closing the current scene. Also shows loading progress in the progress bar.
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    private IEnumerator LoadAsynchronously(string sceneName)
    {
        LoadingInfoObject.SetActive(true); // Activate game object that shows loading progress
        
        // AsyncOperation - Returns an object with information about the progress of loading a new scene
        // SceneManager.LoadSceneAsync - Loads a new scene in the background without closing the current scene
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone) // While scene is loading
        {
            // Divide operation progress by 0.9 because it can be reached up to 0.9
            // On the output we get operation progress value from 0 to 1
            // To be safe clamp this progress from 0 to 1
            float loadingProgress = Mathf.Clamp01(operation.progress / 0.9f);

            loadingProgressBar.value = loadingProgress; // Set the loading progress to the loading bar value
            LoadingProgressText.GetComponent<Text>().text = (int)(loadingProgress * 100) + "%"; // Set the loading progress text (0% - 100%)

            if (loadingProgress == 1) // If loading is done
            {
                LoadingProgressText.GetComponent<Text>().text = "Loading..."; // Change text on loading progress text component
            }
            yield return null; // Wait for the next frame. Thus a while loop will be executed each frame
        }
        yield break; // Exit from this iterator
    }
}
