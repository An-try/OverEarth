using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Class for any UI button that has text or image component.
/// </summary>
public class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Text buttonText;
    private Image buttonImage;

    private Color32 neutralButtonColor; // Button color when it is in default state
    private Color32 activeButtonColor; // Button color when the user hover cursor on it or press the button

    private Vector3 defaulButtontScale;
    private float buttonPressedScaleMultiplier = 0.9f; // Button scale multiplier when button is pressed

    private bool buttonPressed = false;

    private void Awake() // Awake is called when the script instance is being loaded
    {
        neutralButtonColor = new Color32(255, 255, 255, 127); // White translucent color
        activeButtonColor = new Color32(255, 255, 255, 255); // White color

        defaulButtontScale = transform.localScale; // Set defaul buttont scale

        if (transform.GetComponent<Text>() != null) // If there is a text component on the button
        {
            buttonText = transform.GetComponent<Text>(); // Get text component
            buttonText.color = neutralButtonColor; // Set text component color
        }

        if (transform.GetComponent<Image>() != null) // If there is an image component on the button
        {
            buttonImage = transform.GetComponent<Image>(); // Get image component
            buttonImage.color = neutralButtonColor; // Set image component color
        }

        InvokeRepeating("UpdateButton", 1f, 1f); // Check and update button state each period of time
    }

    private void UpdateButton()
    {
        if (!gameObject.activeInHierarchy) // If game object is active in the scene
        {
            SetButtonColor(neutralButtonColor);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) // Called when the cursor enters the zone of the button
    {
        SetButtonColor(activeButtonColor);
    }

    public void OnPointerExit(PointerEventData eventData) // Called when the cursor leaves the button area
    {
        if (!buttonPressed) // If button is not pressed
        {
            SetButtonColor(neutralButtonColor);
        }
    }

    public void OnPointerDown(PointerEventData eventData) // Called when the user presses the left mouse button
    {
        SetButtonPressedScale();
        buttonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData) // Called when the user releases the left mouse button
    {
        SetButtonColor(neutralButtonColor);
        SetButtonDefaultScale();
        buttonPressed = false;
    }

    private void SetButtonPressedScale()
    {
        // Make button smaller by multiplying its each scale value
        transform.localScale = new Vector3(transform.localScale.x * buttonPressedScaleMultiplier,
                                           transform.localScale.y * buttonPressedScaleMultiplier,
                                           transform.localScale.z * buttonPressedScaleMultiplier);
    }

    private void SetButtonDefaultScale()
    {
        transform.localScale = defaulButtontScale; // Set default button scale to this button
    }

    private void SetButtonColor(Color32 newColor)
    {
        if (buttonText != null) // If there is a text component
        {
            buttonText.color = newColor; // Set the new color to the text
        }
        if (buttonImage != null) // If there is an image component
        {
            buttonImage.color = newColor; // Set the new color to the image
        }
    }
}
