using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for methods that can be used in different scenes of the game
/// </summary>
public static class Methods
{
    /// <summary>
    /// Coroutine to activate some game object with text for a while.
    /// </summary>
    /// <param name="WarningObject"></param>
    /// <param name="message"></param>
    /// <param name="showTime"></param>
    /// <returns></returns>
    public static IEnumerator ShowWarningMessage(GameObject WarningObject, string message, float showTime)
    {
        WarningObject.GetComponent<Text>().text = message; // Set the message to text component on this game object
        WarningObject.SetActive(true); // Activate the game object

        yield return new WaitForSeconds(showTime); // Wait some time

        WarningObject.SetActive(false); // Deactivate the game object
        WarningObject.GetComponent<Text>().text = null; // Delete text on component
    }
}
