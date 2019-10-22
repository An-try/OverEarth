using UnityEngine;
using UnityEngine.UI;

public class ResizeCells : MonoBehaviour
{
    private Vector2 screenResolution;

    private void Awake() // Awake is called when the script instance is being loaded
    {
        screenResolution = new Vector2(Screen.width, Screen.height); // Get current screen resolution
    }

    private void FixedUpdate() // FixedUpdate is called at a fixed framerate frequency
    {
        // If this game object is active in the scene and current screen resolution is different than saved resolution
        if (gameObject.activeInHierarchy && (screenResolution.x != Screen.width || screenResolution.y != Screen.height))
        {
            Resize(); // Resize children cells
        }
    }

    private void Resize()
    {
        float width = GetComponent<RectTransform>().rect.width; // Get current width of rect tranform of this game object
        Vector2 newSize = new Vector2(width / 2.6f, width / 2.6f); // Create a new size for the cells
        // Apply new size for GridLayoutGroup component of this game object. It will change size of all children cells
        GetComponent<GridLayoutGroup>().cellSize = newSize;

        screenResolution = new Vector2(Screen.width, Screen.height); // Get current screen resolution
    }
}
