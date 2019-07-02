using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class displays game control
/// </summary>
public class InputHelp : MonoBehaviour
{
    public Text helpText;

    private void Start() // Start is called on the frame when a script is enabled just before any of the Update methods are called the first time
    {
        helpText.text += "Controls:\n" +
            "Show / hide cursor: <color=blue> LeftAlt </color>" +
            "\nSpeed up / down: <color=blue> + </color> / <color=blue> - </color>" +
            "\nStreif up / down: <color=blue> Left_Shift </color> / <color=blue> Left_Ctrl </color>" +
            "\nStrafe left / right: <color=blue> Z </color> / <color=blue> X </color>" +
            "\nRotate left / right: <color=blue> A </color> / <color=blue> D </color>" +
            "\nTurn down / up: <color=blue> W </color> / <color=blue> S </color>" +
            "\nRotation around axis: <color=blue> Q </color> / <color=blue> E </color>" +
            "\nBraking: <color=blue> Space </color>" +
            "\nAuto fire: <color=blue> F </color>" +
            "\nInventory: <color=blue> I </color>" +
            "\nInput help panel: <color=blue> P </color>" +
            "\nChange camera: <color=blue> L </color>" +
            "\nSpawn enemies: <color=blue> 8 </color> / <color=blue> 9 </color> / <color=blue> 0 </color>" +
            "\nPause menu: <color=blue> Esc </color>";
    }
}